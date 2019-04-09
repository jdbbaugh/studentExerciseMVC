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
    public class CohortsController : Controller
    {
        private readonly IConfiguration _configuration;

        public CohortsController(IConfiguration configuration)
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

        // GET: Cohorts
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, cohortName FROM Cohort";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();

                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("cohortName"))
                        };
                        cohorts.Add(cohort);
                    }

                    reader.Close();
                    return View(cohorts);
                }
            }
        }

        // GET: Cohorts/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT id, cohortName FROM Cohort WHERE id = {id}";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Cohort cohort = null;
                    if (reader.Read())
                    {
                        cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("cohortName"))
                        };
                    }
                    reader.Close();
                    return View(cohort);
                }
            }
        }

        // GET: Cohorts/Create
        public ActionResult Create()
        {
            CohortCreateViewModel viewModel = new CohortCreateViewModel();
            return View(viewModel);
        }

        // POST: Cohorts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CohortCreateViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Cohort (CohortName)
                                            VALUES (@cohortName)";
                        cmd.Parameters.Add(new SqlParameter("@cohortName", viewModel.Cohort.Name));

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

        // GET: Cohorts/Edit/5
        public ActionResult Edit(int id)
        {
            Cohort cohort = GetCohortById(id);
            if (cohort == null)
            {
                return NotFound();
            }

            CohortEditViewModel viewModel = new CohortEditViewModel
            {
                Cohort = cohort
            };

            return View(viewModel);
        }

        // POST: Cohorts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CohortEditViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Cohort
                                            SET cohortName = @CohortName
                                            WHERE id = @id";
                        cmd.Parameters.Add(new SqlParameter("@CohortName", viewModel.Cohort.Name));
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

        // GET: Cohorts/Delete/5
        public ActionResult Delete(int id)
        {
            Cohort cohort = GetCohortById(id);
            if (cohort == null)
            {
                return NotFound();
            }

            CohortDeleteViewModel viewModel = new CohortDeleteViewModel
            {
                Cohort = cohort
            };

            return View(viewModel);
        }

        // POST: Cohorts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CohortDeleteViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Cohort WHERE id = @id";
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

        private Cohort GetCohortById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, cohortName FROM Cohort WHERE id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Cohort cohort = null;

                    if (reader.Read())
                    {
                        cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("cohortName"))
                        };
                    }
                    reader.Close();
                    return cohort;
                }
            }
        }

    }
}