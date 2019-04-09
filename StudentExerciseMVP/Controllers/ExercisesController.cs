using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExerciseMVP.Models;
using StudentExerciseMVP.Models.ViewModels;

namespace StudentExerciseMVP.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly IConfiguration _configuration;

        public ExercisesController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }

        }
        // GET: Exercises
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, exerciseName, exerciseLanguage FROM Exercise";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();

                    while (reader.Read())
                    {
                        Exercise exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("exerciseName")),
                            ExerciseLanguage = reader.GetString(reader.GetOrdinal("exerciseLanguage"))
                        };
                        exercises.Add(exercise);
                    }
                    reader.Close();
                    return View(exercises);
                }
            }
        }

        // GET: Exercises/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT id, exerciseName, exerciseLanguage FROM Exercise WHERE id ={id}";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Exercise exercise = null;
                    if (reader.Read())
                    {
                        exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("exerciseName")),
                            ExerciseLanguage = reader.GetString(reader.GetOrdinal("exerciseLanguage"))
                        };
                    }
                    reader.Close();
                    return View(exercise);
                }
            }
        }

        // GET: Exercises/Create
        public ActionResult Create()
        {
            ExerciseCreateViewModel viewModel = new ExerciseCreateViewModel();
            return View(viewModel);
        }

        // POST: Exercises/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExerciseCreateViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Exercise (ExerciseName, ExerciseLanguage)
                                            VALUES (@exerciseName, @exerciseLanguage)";
                        cmd.Parameters.Add(new SqlParameter("@exerciseName",viewModel.Exercise.ExerciseName));
                        cmd.Parameters.Add(new SqlParameter("@exerciseLanguage",viewModel.Exercise.ExerciseLanguage));

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Exercises/Edit/5
        public ActionResult Edit(int id)
        {
            Exercise exercise = GetExerciseById(id);
            if (exercise == null)
            {
                return NotFound();
            }

            ExerciseEditViewModel viewModel = new ExerciseEditViewModel
            {
                Exercise = exercise
            };

            return View(viewModel);
        }

        // POST: Exercises/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ExerciseEditViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Exercise
                                            SET exerciseName = @exerciseName,
                                                exerciseLanguage = @exerciseLanguage
                                            WHERE id = @id";
                        cmd.Parameters.Add(new SqlParameter("@exerciseName", viewModel.Exercise.ExerciseName));
                        cmd.Parameters.Add(new SqlParameter("@exerciseLanguage", viewModel.Exercise.ExerciseLanguage));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Exercises/Delete/5
        public ActionResult Delete(int id)
        {
            Exercise exercise = GetExerciseById(id);
            if (exercise == null)
            {
                return NotFound();
            }


            ExerciseDeleteViewModel viewModel = new ExerciseDeleteViewModel
            {
                Exercise = exercise
            };

            return View(viewModel);
        }

        // POST: Exercises/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ExerciseDeleteViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM exercise WHERE id = @id;";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

        }
            private Exercise GetExerciseById(int id)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT Id, ExerciseName, ExerciseLanguage
                                            FROM Exercise
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        SqlDataReader reader = cmd.ExecuteReader();

                        Exercise exercise = null;

                        if (reader.Read())
                        {
                            exercise = new Exercise
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ExerciseName = reader.GetString(reader.GetOrdinal("ExerciseName")),
                                ExerciseLanguage = reader.GetString(reader.GetOrdinal("ExerciseLanguage"))
                            };
                        }
                        reader.Close();
                        return exercise;
                    }
                }
            }
    }
}