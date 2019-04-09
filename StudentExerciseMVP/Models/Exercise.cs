using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExerciseMVP.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Exercise Name")]
        public string ExerciseName { get; set; }

        [Required]
        [Display(Name = "Language")]
        public string ExerciseLanguage { get; set; }
    }
}
