using DataLayer.Models;
using System.Collections.Generic;
namespace DataLayer.ViewModels
{
    public class DFieldVm
    {
        /// <summary>
        /// Field Id
        /// <see cref="Models.TblField"/>
        /// </summary>
        public int FieldId { get; set; }
        /// <summary>
        /// Owner Forms Id
        /// <see cref="Models.TblForm"/>
        /// </summary>
        public int FormId { get; set; }
        /// <summary>
        /// Field Name
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// The type of this field
        /// <see cref="DFieldType"/>
        /// </summary>
        public DFieldType Type { get; set; }
        /// <summary>
        /// Is the field Required
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// Options for multi select inputs like Dropdowns and RadioButtons
        /// <example>"Apple,Carrots,Annanas,Orange,Blueberry"</example>
        /// </summary>
        public string Options { get; set; }
        /// <summary>
        /// Shown if the input supports showing placeholders
        /// </summary>
        public string Placeholder { get; set; }
        /// <summary>
        /// To provide extra information about the input field
        /// </summary>
        public string Tooltip { get; set; }
        /// <summary>
        /// Don't show the field if IsDeleted is true
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Static Validation Rules
        /// </summary>
        public List<DRegexVm> Validations { get; set; }

        public DFieldVm()
        {

        }

        public DFieldVm(int fieldId, int formId, string label, DFieldType type, bool isRequired, string options, string placeholder, string tooltip, List<DRegexVm> validations, bool isDeleted = false)
        {
            FieldId = fieldId;
            FormId = formId;
            Label = label;
            Type = type;
            IsRequired = isRequired;
            Options = options;
            Placeholder = placeholder;
            Tooltip = tooltip;
            IsDeleted = isDeleted;
            Validations = validations;
        }

        public DFieldVm(TblField tblField, List<DRegexVm> validations, int formId)
        {
            FormId = formId;
            FieldId = tblField.FieldId;
            Label = tblField.Label;
            Type = (DFieldType)int.Parse(tblField.Type);
            IsRequired = tblField.IsRequired.Value;
            Options = tblField.Options;
            Placeholder = tblField.Placeholder;
            Tooltip = tblField.Tooltip;
            IsDeleted = tblField.IsDeleted;
            Validations = validations;
        }

    }
}
