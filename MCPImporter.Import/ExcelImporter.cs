using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel;
using LinqToExcel.Domain;
using LinqToExcel.Query;
using MCPImporter.Business;
using MCPImporter.Business.Application;
using MCPImporter.Common.Entities;

namespace MCPImporter.Import
{
    public class ExcelImporter : IImporter
    {
        #region Constants

        private const string ORG_PARTNER_ID = "Organization Partner ID",
                             LOCATION_ID = "Location - PartnerID",
                             LOCATION_NAME = "Location Name",
                             LOCATION_COUNTRY = "Location Country",
                             MCP_ID = "MCP ID",
                             LAST_NAME = "Last Name",
                             FIRST_NAME = "First Name",
                             EMAIL = "E-mail",
                             CERT_TRACK = "Certification Track",
                             CERT_NAME = "Certification Name",
                             QUAL_COMP = "Qualifying Competencies",
                             ASSIGNED_COMP = "Competency Currently Assigned to",
                             STATUS = "Status";


        #endregion

        #region Fields

        readonly IApplicationBCFactory _factory;
        readonly IEntityBC<Location> _locationBC;
        readonly IEntityBC<Organization> _organizationBC;
        readonly IEntityBC<Status> _statusBC;
        readonly IEntityBC<CertificationTrack> _certificationTrackBC;
        readonly IEntityBC<CertificationName> _certificationNameBC;
        readonly IEntityBC<QualifyingCompetency> _qualifyingCompetencyBC;
        readonly IEntityBC<AssignedCompetency> _assignedCompetencyBC;
        readonly IEntityBC<Person> _personBC;

        #endregion

        #region Properties

        FileInfo ImportFileInfo { get; set; }

        ExcelQueryFactory Excel { get; set; }
        string WorksheetName { get; set; }

        private IList<Status> Statuses { get; set; }
        private IList<AssignedCompetency> AssignedCompetencies { get; set; }
        private IList<CertificationTrack> CertificationTracks { get; set; }
        private IList<CertificationName> CertificationNames { get; set; }
        private IList<QualifyingCompetency> QualifyingCompetencies { get; set; }
        private IList<Organization> Organizations { get; set; }
        private IList<Location> Locations { get; set; }
        private IList<Person> Persons { get; set; } 

        #endregion

        #region ctor

        public ExcelImporter()
        {
            _factory = new ApplicationBCFactory();
            _locationBC = _factory.LocationBC;
            _organizationBC = _factory.OrganizationBC;

            _statusBC = _factory.StatusBC;
            _certificationTrackBC = _factory.CertificationTrackBC;
            _certificationNameBC = _factory.CertificationNameBC;
            _qualifyingCompetencyBC = _factory.QualifyingCompetencyBC;
            _assignedCompetencyBC = _factory.AssignedCompetencyBC;
            _personBC = _factory.PersonBC;
        }

        #endregion

        #region Interface Implementation

        public string GetConnectionString(string fileName)
        {
            string excelDataConnectionString = String.Empty;
            string importFileLocation = fileName;

            ImportFileInfo = new System.IO.FileInfo(importFileLocation);
            if (!ImportFileInfo.Exists)
            {
                return null;
            }

            const string header = "HDR=YES;";

            switch (ImportFileInfo.Extension.ToUpper())
            {
                case ".XLSX":
                    excelDataConnectionString = String.Format(
                        "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;{1}\"",
                        importFileLocation,
                        header);

                    break;
                case ".XLS":
                    excelDataConnectionString = String.Format(
                         "Provider=Microsoft.Jet.OleDb.4.0;Data Source={0};Extended Properties=\"Excel 8.0;{1}\";",
                         importFileLocation,
                         header);
                    break;
            }

            return excelDataConnectionString;
        }

        public async Task<bool> ExtractInformation(string connectionString)
        {
            try
            {
                Excel = new ExcelQueryFactory(connectionString);
                WorksheetName = Excel.GetWorksheetNames().First();

                #region
                /*
                //var statuses = await ImportEntities(excel.Worksheet<Status>(worksheet), statusBC, () => MapAssigned(excel));
                //var statuses = await ImportEntities(excel.Worksheet<Status>(worksheet), statusBC);
                //var assignedComp = await ImportEntities(excel.Worksheet<AssignedCompetency>(worksheet), assignedCompetencyBC);

                //var certTracks = await ImportCombinedEntities(excel.Worksheet<CertificationTrack>(worksheet), certificationTrackBC);
                //var certNames = await ImportCombinedEntities(excel.Worksheet<CertificationName>(worksheet), certificationNameBC);
                //var qualComp = await ImportCombinedEntities(excel.Worksheet<QualifyingCompetency>(worksheet), qualifyingCompetencyBC);

                //var organizations = await ImportOrganizations(excel.Worksheet<Organization>(worksheet));
                //var locations = await ImportLocations(excel.Worksheet<Location>(worksheet), organizations.First());

                //var people = excel.Worksheet<Person>(worksheet).Select(person => person).ToList();

                //var org = excel.Worksheet<Organization>(worksheet).Select(organization => organization).ToList();
                */
                #endregion

                Statuses = await ImportEntities(_statusBC);
                AssignedCompetencies = await ImportEntities(_assignedCompetencyBC);

                CertificationTracks = await ImportCombinedEntities(_certificationTrackBC);
                CertificationNames = await ImportCombinedEntities(_certificationNameBC);
                QualifyingCompetencies = await ImportCombinedEntities(_qualifyingCompetencyBC);

                Organizations = await ImportOrganizations();
                Locations = await ImportLocations();

                Persons = await ImportPersons();

                if (NumberOfPeopleImported != null)
                    NumberOfPeopleImported(Persons.Count.ToString());
            }
            catch (Exception ex)
            {
                if (NumberOfPeopleImported != null)
                    NumberOfPeopleImported("ERROR!");

                return false;
            }

            return true;
        }

        public Action<string> NumberOfPeopleImported { get; set; }

        #endregion

        #region Import Entities

        private async Task<IList<T>> ImportEntities<T>(IEntityBC<T> bc) where T : ILookupValue, new()
        {
            MapColumns<T>(this.Excel);

            var distinct = new List<T>();

            distinct.AddRange(this.Excel.Worksheet<T>(this.WorksheetName).ToList().Where(item => !distinct.Select(x => x.Name).Contains(item.Name)));

            bc.Add(distinct);
            await bc.Save();

            return await bc.GetAll();
        }

        private async Task<IList<T>> ImportCombinedEntities<T>(IEntityBC<T> bc) where T : ILookupValue, new()
        {
            MapColumns<T>(this.Excel);

            var distinct = new List<T>();

            distinct.AddRange(
                this.Excel.Worksheet<T>(this.WorksheetName).ToList()
                    .Select(item => item.Name.Split(','))
                    .SelectMany(values => values.Where(value => !distinct
                                                                   .Select(x => x.Name)
                                                                   .Contains(value)))
                    .Select(x => new T { Name = x })
                );

            bc.Add(distinct);
            await bc.Save();

            return await bc.GetAll();
        }

        private async Task<IList<Organization>> ImportOrganizations()
        {
            MapOrganization(this.Excel);

            var distinct = new List<Organization>();

            distinct.AddRange(this.Excel.Worksheet<Organization>(this.WorksheetName).ToList().Where(item => !distinct.Select(x => x.OrgPartnerId).Contains(item.OrgPartnerId)));

            _organizationBC.Add(distinct);

            await _organizationBC.Save();

            return await _organizationBC.GetAll();
        }

        private async Task<IList<Location>> ImportLocations()
        {
            MapLocation(this.Excel);

            var distinct = new List<Location>();

            distinct.AddRange(this.Excel.Worksheet<Location>(this.WorksheetName).ToList().Where(item => !distinct.Select(x => x.Name).Contains(item.Name)));

            var org = Organizations.First();

            distinct.ForEach(x => x.Organization = org);

            _locationBC.Add(distinct);

            await _locationBC.Save();

            return await _locationBC.GetAll();
        }

        private async Task<IList<Person>> ImportPersons()
        {
            var allRows = this.Excel.Worksheet(this.WorksheetName).ToList();

            foreach (var row in allRows)
            {
                var person = new Person();

                //have to manually map the person as it can't cast for some reason
                person.MCPId = row[MCP_ID].Cast<string>();
                person.LastName = row[LAST_NAME].Cast<string>();
                person.FirstName = row[FIRST_NAME].Cast<string>();
                person.Email = row[EMAIL].Cast<string>();

                person.AssignedCompetency = AssignedCompetencies.OrderBy(x => x.Id).LastOrDefault(x => x.Name == row[ASSIGNED_COMP]);
                person.Status = Statuses.OrderBy(x => x.Id).LastOrDefault(x => x.Name == row[STATUS]);
                person.Location = Locations.OrderBy(x => x.Id).LastOrDefault(x => x.PartnerId == row[LOCATION_ID]);

                foreach (var track in CertificationTracks.Where(x => row[CERT_TRACK].Cast<string>().Split(',').Contains(x.Name)))
                {
                    person.CertificationTracks.Add(track); 
                }
                foreach (var name in CertificationNames.Where(x => row[CERT_NAME].Cast<string>().Split(',').Contains(x.Name)))
                {
                    person.CertificationNames.Add(name);
                }
                foreach (var qual in QualifyingCompetencies.Where(x => row[QUAL_COMP].Cast<string>().Split(',').Contains(x.Name)))
                {
                    person.QualifyingCompetencies.Add(qual);
                }

                _personBC.Add(person);
            }

            await _personBC.Save();

            return await _personBC.GetAll();
        }

        #endregion

        #region Mappings

        private void MapColumns<T>(ExcelQueryFactory factory) where T : new()
        {
            var obj = new T();

            if (obj is Organization)
            {
                MapOrganization(factory);
            }
            else if (obj is Location)
            {
                MapLocation(factory);
            }
            else if (obj is Status)
            {
                MapStatus(factory);
            }
            else if (obj is Person)
            {
                MapPerson(factory);
            }
            else if (obj is AssignedCompetency)
            {
                MapAssigned(factory);
            }
            else if (obj is CertificationTrack)
            {
                MapCertTrack(factory);
            }
            else if (obj is CertificationName)
            {
                MapCertName(factory);
            }
            else if (obj is QualifyingCompetency)
            {
                MapQualifying(factory);
            }
        }

        private void MapOrganization(ExcelQueryFactory factory)
        {
            factory.AddMapping<Organization>(x => x.OrgPartnerId, ORG_PARTNER_ID);
        }
        private void MapLocation(ExcelQueryFactory factory)
        {
            factory.AddMapping<Location>(location => location.PartnerId, LOCATION_ID);
            factory.AddMapping<Location>(location => location.Name, LOCATION_NAME);
            factory.AddMapping<Location>(location => location.Country, LOCATION_COUNTRY);
        }
        private void MapPerson(ExcelQueryFactory factory)
        {
            factory.AddMapping<Person>(person => person.MCPId, MCP_ID);
            factory.AddMapping<Person>(person => person.LastName, LAST_NAME);
            factory.AddMapping<Person>(person => person.FirstName, FIRST_NAME);
            factory.AddMapping<Person>(person => person.Email, EMAIL);
            factory.AddMapping("Name", "");
        }
        private void MapAssigned(ExcelQueryFactory factory)
        {
            factory.AddMapping<AssignedCompetency>(competency => competency.Name, ASSIGNED_COMP);
        }
        private void MapStatus(ExcelQueryFactory factory)
        {
            factory.AddMapping<Status>(status => status.Name, STATUS);
        }
        private void MapCertTrack(ExcelQueryFactory factory)
        {
            factory.AddMapping<CertificationTrack>(track => track.Name, CERT_TRACK);
        }
        private void MapCertName(ExcelQueryFactory factory)
        {
            factory.AddMapping<CertificationName>(name => name.Name, CERT_NAME);
        }
        private void MapQualifying(ExcelQueryFactory factory)
        {
            factory.AddMapping<QualifyingCompetency>(q => q.Name, QUAL_COMP);
        }
        #endregion
    }
}
