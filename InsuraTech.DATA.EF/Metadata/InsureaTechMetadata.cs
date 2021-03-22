using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InsuraTech.DATA.EF.Metadata
{
    /*
       *General rules and standard practices for metadata
       * 1) All metadata can exist in a single file for all classes associated to Entity Framework.
       * 2) Or, it can be split between files for each class associated to Entity Framework.
       * We will be doing (1) in our examples.
       * 3) Metadata classes must be in the same namespace as the original EF class.
       * 4) Short guide for connecting each pair of metadata classes and EF classes.
       * 
       *  a) ensure that the namespaces of the files match (match the .tt namespace). Add the using for System.ComponentModel.DataAnnotations
       *  
       *  b) Create the metadata class (empty)
       *      ex- public class MyTableMetadata{}
       *      
       *  c) Apply the metadata type attribute to the meatadata partial class
       *      ex- [MetadataType(typeof(MyTableMetadata))]
       *      
       *  d) Create the metadata partial class with the same exact name as the EF class
       *      ex- public partial class MyTable{}
       *      
       *  e) Use the EF class and copy the properties to our metadata class
       *      ex- public class MyTableMetadata
       *      {
       *          public int MyTableID { get; set; }
       *          public string MyField { get; set; }
       *       }
       *     
       *  f) Apply all necessary metadata attributes
       *      ex- public class MyTableMetadata
       *      {
       *          [Required(ErrorMessage = "*")]
       *          public int MyTableID { get; set; }
       *          [Display(Name ="My Field")]
       *          public string MyField { get; set; }
       *       }
       *       
       *  5) Use the diagram from SSMS to ensure that all validation and metadata is correct.     
       */


    /*
     *Notes for MetaData Attributes
     * 1) If a DB column name changes in the DB then the associated Metadata names need to change too.
     * 2) Primary keys dont actually need annotations
     * 3) We can change how the DB field names are displayed in the UI using:[Display(Name = "First Name")]
     * 4) We want to use the DB to know the field length and if Null's are allowed.*/


    /*------------------------------------------------------------------------------------------------------*/




    #region Applications
    public class ApplicationMetadata
    {
        [Required(ErrorMessage = "*Open Position ID is required")]
        [Display(Name = "Open Position ID")]
        public int OpenPositionId { get; set; }

        [StringLength(128, ErrorMessage = "*Value must be 128 charachters or less.")]
        [Required(ErrorMessage = "*User ID is required")]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Display(Name = "Application Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "[N/A]")]
        public System.DateTime ApplicationDate { get; set; }

        [StringLength(2000, ErrorMessage = "*Value must be 2000 charachters or less.")]
        [Display(Name = "Manager Notes")]
        public string ManagerNotes { get; set; }

        [Required(ErrorMessage = "*Application Status is required")]
        [Display(Name = "Application Status")]
        public int ApplicationStatus { get; set; }


        [StringLength(75, ErrorMessage = "*Value must be 75 charachters or less.")]
        [Display(Name = "Resume File Name")]
        public string ResumeFileName { get; set; }

    }

    [MetadataType(typeof(ApplicationMetadata))]
    public partial class Application { }


    #endregion

    #region ApplicationStatu
    public class ApplicationStatuMetadata
    {
        [StringLength(50, ErrorMessage = "*Value must be 50 charachters or less.")]
        [Required(ErrorMessage = "*User ID is required")]
        [Display(Name = "Status Name")]
        public string StatusName { get; set; }

        [StringLength(250, ErrorMessage = "*Value must be 250 charachters or less.")]
        [Display(Name = "Status Description")]
        public string StatusDescription { get; set; }
    }
    [MetadataType(typeof(ApplicationStatuMetadata))]
    public partial class ApplicationStatu { }

    #endregion

    #region Location
    public class LocationMetadata
    {
        [StringLength(10, ErrorMessage = "*Value must be 10 charachters or less.")]
        [Required(ErrorMessage = "*Store Number is required")]
        [Display(Name = "Store Number")]
        public string StoreNumber { get; set; }

        [StringLength(10, ErrorMessage = "*Value must be 10 charachters or less.")]
        [Required(ErrorMessage = "*City is required")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "*State is required")]
        [Display(Name = "State")]
        public char State { get; set; }


        [StringLength(128, ErrorMessage = "*Value must be 128 charachters or less.")]
        [Required(ErrorMessage = "*Manager ID is required")]
        [Display(Name = "Manager ID")]
        public string ManagerId { get; set; }

    }
    [MetadataType(typeof(LocationMetadata))]
    public partial class Location { }
    #endregion

    #region MyRegion
    public class OpenPositionMetadata
    {
        [Required(ErrorMessage = "*Position ID is required")]
        [Display(Name = "Position ID")]
        public int PositionId { get; set; }

        [Required(ErrorMessage = "*Location ID is required")]
        [Display(Name = "Location ID")]
        public int LocationId { get; set; }
    }
    [MetadataType(typeof(OpenPositionMetadata))]
    public partial class OpenPosition { }
    #endregion

    #region Position
    public class PositionMetadata
    {
        [StringLength(50, ErrorMessage = "*Value must be 50 charachters or less.")]
        [Required(ErrorMessage = "*Job Title is required")]
        [Display(Name = "Job Title")]
        public string Title { get; set; }

        [StringLength(200, ErrorMessage = "*Value must be 200 charachters or less.")]
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
    }
    [MetadataType(typeof(PositionMetadata))]
    public partial class Position { }

    #endregion

    #region UserDetail
    public class UserDetailMetadata
    {

        [StringLength(50, ErrorMessage = "*Value must be 50 charachters or less.")]
        [Required(ErrorMessage = "*First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "*Value must be 50 charachters or less.")]
        [Required(ErrorMessage = "*Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(75, ErrorMessage = "*Value must be 75 charachters or less.")]
        [Display(Name = "Upload Resume")]
        public string ResumeFileName { get; set; }
    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail { }
    #endregion



}//end namespace
