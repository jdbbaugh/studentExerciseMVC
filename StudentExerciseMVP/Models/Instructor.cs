using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExerciseMVP.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "Instructor Name")]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }


        [Required]
        [Display(Name = "Slack")]
        public string SlackHandle { get; set; }
        [Required]
        [Display(Name = "Cohort")]
        private int CohortId { get; set; }
        public Cohort Cohort { get; set; }
    }

}
