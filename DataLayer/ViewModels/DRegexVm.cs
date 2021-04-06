using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataLayer.ViewModels
{
    public class DRegexVm
    {
        public int RegexId { get; set; }
        /// <summary>
        /// Regular Expression string
        /// </summary>
        public string Regex { get; set; }
        /// <summary>
        /// Message to show when the validation fails
        /// </summary>
        public string ValidationMessage { get; set; }
        /// <summary>
        /// A familiar name for the regex Rule
        /// </summary>
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public DRegexVm()
        {

        }

        public DRegexVm(int regexId, string name, string regex, string validationMessage, bool isDeleted = false)
        {
            RegexId = regexId;
            Regex = regex;
            ValidationMessage = validationMessage;
            IsDeleted = isDeleted;
            Name = name;
        }
    }
}
