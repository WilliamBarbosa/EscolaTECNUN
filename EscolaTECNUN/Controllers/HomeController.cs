using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using EscolaTECNUN.Models;
using System.Configuration;
using Newtonsoft.Json;

namespace EscolaTECNUN.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Aluno()
        {
            ViewBag.Message = "Página do Aluno";

            return View();
        }

        public ActionResult Professor()
        {
            ViewBag.Message = "Página do Professor.";

            return View();
        }

        public ActionResult Turma()
        {
            ViewBag.Message = "Página da Turma";

            return View();
        }

        public ActionResult Matricula()
        {
            ViewBag.Message = "Página de Matricula";

            return View();
        }

        [HttpPost]  
        public ActionResult CadAluno(Aluno aluno)
        {

            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);
            //definição do comando sql
            string sql = "INSERT INTO Aluno(nome, datanasc, telefone, cpf, email, infoadic) " +
                "VALUES(@nome, @datanasc, @telefone, @cpf, @email, @infoadic)";


            try
            {
                SqlCommand comando = new SqlCommand(sql, conn);

                comando.Parameters.Add(new SqlParameter("@nome", aluno.Nome));
                comando.Parameters.Add(new SqlParameter("@datanasc", aluno.DataNasc));
                comando.Parameters.Add(new SqlParameter("@telefone", aluno.Telefone));
                comando.Parameters.Add(new SqlParameter("@cpf", aluno.CPF));
                comando.Parameters.Add(new SqlParameter("@email", aluno.Email));
                comando.Parameters.Add(new SqlParameter("@infoadic", aluno.InfoAdic));

                conn.Open();
                comando.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception ex)
            {
                return Content("Erro Ao Cadastrar Aluno");
            }
            finally
            {
                conn.Close();
            }

            return Content("Cadastrado com Sucesso");
        }

        [HttpPost]
        public ActionResult CadProfessor(Professor professor)
        {

            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);
            //definição do comando sql
            string sql = "INSERT INTO Professor(nome, datanasc, telefone, cpf) VALUES(@nome, @datanasc, @telefone, @cpf)";


            try
            {
                SqlCommand comando = new SqlCommand(sql, conn);

                comando.Parameters.Add(new SqlParameter("@nome", professor.Nome));
                comando.Parameters.Add(new SqlParameter("@datanasc", professor.DataNasc));
                comando.Parameters.Add(new SqlParameter("@telefone", professor.Telefone));
                comando.Parameters.Add(new SqlParameter("@cpf", professor.CPF));

                conn.Open();
                comando.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception ex)
            {
                return Content("Erro Ao Cadastrar Professor");
            }
            finally
            {
                conn.Close();
            }

            return Content("Cadastrado com Sucesso");
        }

        [HttpPost]
        public ActionResult CadTurma(Turma turma)
        {

            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);

            string sql = "SELECT TOP (1) id FROM dbo.Professor WHERE id = @id";

            SqlCommand comando = new SqlCommand(sql, conn);

            conn.Open();

            comando.Parameters.Add(new SqlParameter("@id", turma.ProfessorId));

            SqlDataReader reader = comando.ExecuteReader();

            if (!reader.HasRows)
            {
                conn.Close();
                return Content("Não existe o professor com id '" + turma.ProfessorId + "' em nossa base de dados");
            }

            conn.Close();

            //definição do comando sql
            sql = "INSERT INTO Turma(numturma, dataturma, periodo, horario, professorid) " +
                "VALUES(@numturma, @dataturma, @periodo, @horario, @professorid)";


            try
            {
                comando = new SqlCommand(sql, conn);

                comando.Parameters.Add(new SqlParameter("@numturma", turma.NumTurma));
                comando.Parameters.Add(new SqlParameter("@dataturma", turma.DataTurma));
                comando.Parameters.Add(new SqlParameter("@periodo", turma.Periodo));
                comando.Parameters.Add(new SqlParameter("@horario", turma.Horario));
                comando.Parameters.Add(new SqlParameter("@professorid", turma.ProfessorId));

                conn.Open();
                comando.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception ex)
            {
                return Content("Erro Ao Cadastrar turma");
            }
            finally
            {
                conn.Close();
            }

            return Content("Cadastrado com Sucesso");
        }

        public ActionResult ListarTurmas()
        {
            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);
            //definição do comando sql
            string sql = "SELECT t.numturma, t.periodo, p.nome " +
                "FROM dbo.Turma t " +
                "INNER JOIN dbo.Professor p ON t.professorid = p.id";

            List<ListaTurma> listaTurma = new List<ListaTurma>();


            try
            {
                SqlCommand comando = new SqlCommand(sql, conn);

                conn.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ListaTurma t = new ListaTurma();
                            t.NumTurma = reader.GetString(reader.GetOrdinal("numturma"));
                            t.Periodo = reader.GetString(reader.GetOrdinal("periodo"));
                            t.Professor = reader.GetString(reader.GetOrdinal("nome"));
                            listaTurma.Add(t);
                        }
                    }
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                return Content("Erro ao listar turmas");
            }
            finally
            {
                conn.Close();
            }

            return View(listaTurma);
        }

        
        public ActionResult MatricularAluno(Matricula matricula)
        {
            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);

            int AlunoId = 0;
            int TurmaId = 0;

            try
            {

                conn.Open();
                //Consultar aluno com o cpf informado
                string sql = "SELECT TOP (1) id FROM dbo.Aluno WHERE cpf = @cpf";

                SqlCommand comando = new SqlCommand(sql, conn);

                comando.Parameters.Add(new SqlParameter("@cpf", matricula.CPF));

                SqlDataReader reader = comando.ExecuteReader();

                if (!reader.HasRows)
                {
                    conn.Close();
                    return Content("Não existe o aluno com CPF '" + matricula.CPF + "' em nossa base de dados");
                }

                if (reader.Read())
                {
                    AlunoId = (int)reader["id"];
                }

                conn.Close();

                //Consultar Turma com o numero informado

                sql = "SELECT TOP (1) id FROM dbo.Turma WHERE numturma = @numturma";

                comando = new SqlCommand(sql, conn);

                conn.Open();

                comando.Parameters.Add(new SqlParameter("@numturma", matricula.NumTurma));

                reader = comando.ExecuteReader();

                if (!reader.HasRows)
                {
                    conn.Close();
                    return Content("A Turma '" + matricula.NumTurma  + "' não existe em nossa base de dados");
                }

                if (reader.Read())
                {
                    TurmaId = (int)reader["id"];
                }

                conn.Close();


                sql = "INSERT INTO Matricula(alunoid, turmaid) VALUES(@alunoid, @turmaid)";

                comando = new SqlCommand(sql, conn);

                conn.Open();

                comando.Parameters.Add(new SqlParameter("@alunoid", AlunoId));
                comando.Parameters.Add(new SqlParameter("@turmaid", TurmaId));

                comando.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception ex)
            {
                conn.Close();
                return Content("Erro ao matricular aluno");
            }
            finally
            {
                conn.Close();
            }

            return Content("Aluno matriculado com Sucesso!");
        }

        public ActionResult VerAlunos(InfoTurma infoturma)
        {
            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);
            //definição do comando sql

            string sql = "";

            if (infoturma.NumTurma == null)
            {
                sql = "SELECT * " +
                " FROM dbo.Aluno ";
            }
            else
            {
                sql = "SELECT a.* " +
                " FROM dbo.Aluno a " +
                " INNER JOIN dbo.Matricula m ON m.alunoid = a.id" +
                " INNER JOIN dbo.Turma t ON t.id = m.turmaid" +
                " WHERE t.numturma = @numturma";
            }

            try
            {
                infoturma.Aluno = new List<Aluno>();

                SqlCommand comando = new SqlCommand(sql, conn);

                if (infoturma.NumTurma != null)
                    comando.Parameters.Add(new SqlParameter("@numturma", infoturma.NumTurma));

                conn.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Aluno a = new Aluno();
                            a.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            a.Nome = reader.GetString(reader.GetOrdinal("nome"));
                            a.CPF = reader.GetString(reader.GetOrdinal("cpf"));
                            a.DataNasc = reader.GetDateTime(reader.GetOrdinal("datanasc"));
                            a.Telefone = reader.GetString(reader.GetOrdinal("telefone"));
                            a.Email = reader.GetString(reader.GetOrdinal("email"));
                            a.InfoAdic = reader.GetString(reader.GetOrdinal("infoadic"));
                            infoturma.Aluno.Add(a);
                        }
                    }
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                return Content("Erro ao visualizar os alunos");
            }
            finally
            {
                conn.Close();
            }

            if (infoturma.NumTurma == null)
                return View("Alunos",infoturma);
            else
                return View("VerAlunos", infoturma);
        }

        public ActionResult ConsultaProfessores()
        {
            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);
            //definição do comando sql
            string sql = "SELECT id, nome " +
                " FROM dbo.Professor";

            List<Professor> professor = new List<Professor>();

            try
            {
                
                SqlCommand comando = new SqlCommand(sql, conn);

                conn.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Professor p = new Professor();
                            p.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            p.Nome = reader.GetString(reader.GetOrdinal("nome"));
                            professor.Add(p);
                        }
                    }
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                return Json(new { 
                    Sucesso = false,
                    Mensagem = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Close();
            }

            return Json(professor, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsultaTurma(string NumTurma)
        {
            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);
            //definição do comando sql
            string sql = "SELECT dataturma, periodo ,horario, professorid" +
                         " FROM dbo.Turma WHERE numturma = '"  + NumTurma + "'";

            Turma turma = new Turma();

            try
            {

                SqlCommand comando = new SqlCommand(sql, conn);

                conn.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            turma.DataTurma = Convert.ToDateTime(reader["dataturma"]);
                            turma.Horario = Convert.ToString(reader["horario"]);
                            turma.Periodo = Convert.ToString(reader["periodo"]);
                            turma.ProfessorId = Convert.ToInt32(reader["professorid"]);
                        }
                    }
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Close();
            }

            return Json(turma, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AtualizarTurma(Turma turma)
        {
            //Turma turma = JsonConvert.DeserializeObject<Turma>(jsonTurma);

            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);

            string sql = "SELECT TOP (1) id FROM dbo.Professor WHERE id = @id";

            SqlCommand comando = new SqlCommand(sql, conn);

            conn.Open();

            comando.Parameters.Add(new SqlParameter("@id", turma.ProfessorId));

            SqlDataReader reader = comando.ExecuteReader();

            if (!reader.HasRows)
            {
                conn.Close();
                return Content("Não existe o professor com id '" + turma.ProfessorId + "' em nossa base de dados");
            }

            conn.Close();

            //definição do comando sql
            sql = "UPDATE Turma SET dataturma =  @dataturma," +
                " periodo = @periodo, horario = @horario, professorid =  @professorid" +
                " WHERE numturma = @numturma";


            try
            {
                comando = new SqlCommand(sql, conn);

                comando.Parameters.Add(new SqlParameter("@numturma", turma.NumTurma));
                comando.Parameters.Add(new SqlParameter("@dataturma", turma.DataTurma));
                comando.Parameters.Add(new SqlParameter("@periodo", turma.Periodo));
                comando.Parameters.Add(new SqlParameter("@horario", turma.Horario));
                comando.Parameters.Add(new SqlParameter("@professorid", turma.ProfessorId));

                conn.Open();
                comando.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    Mensagem = "Erro Ao Atualizar turma"
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Close();
            }

            return Json(new
            {
                Sucesso = true,
                Mensagem = "Alterado com Sucesso"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoverTurma(string NumTurma)
        {
            //Turma turma = JsonConvert.DeserializeObject<Turma>(jsonTurma);

            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);

            try
            {
                string sql = "DELETE FROM Matricula WHERE turmaid = " +
                "(SELECT id FROM Turma WHERE numturma = @numturma)";

                SqlCommand comando = new SqlCommand(sql, conn);

                comando.Parameters.Add(new SqlParameter("@numturma", NumTurma));

                conn.Open();
                comando.ExecuteNonQuery();
                conn.Close();

                //definição do comando sql
                sql = "DELETE FROM Turma WHERE numturma = @numturma";

                comando = new SqlCommand(sql, conn);

                comando.Parameters.Add(new SqlParameter("@numturma", NumTurma));

                conn.Open();
                comando.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    Mensagem = "Erro Ao Remover turma"
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Close();
            }

            return Json(new
            {
                Sucesso = true,
                Mensagem = "Removido com Sucesso"
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ConsultaAluno(string CodAluno)
        {
            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);
            //definição do comando sql
            string sql = "SELECT nome, datanasc ,telefone, cpf, email, infoadic" +
                         " FROM dbo.Aluno WHERE id = " + CodAluno;

            Aluno aluno = new Aluno();

            try
            {

                SqlCommand comando = new SqlCommand(sql, conn);

                conn.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            aluno.Nome = Convert.ToString(reader["nome"]);
                            aluno.CPF = Convert.ToString(reader["cpf"]);
                            aluno.DataNasc = Convert.ToDateTime(reader["datanasc"]);
                            aluno.Telefone = Convert.ToString(reader["telefone"]);
                            aluno.Email = Convert.ToString(reader["email"]);
                            aluno.InfoAdic = Convert.ToString(reader["infoadic"]);
                        }
                    }
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Close();
            }

            return Json(aluno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AtualizarAluno(Aluno aluno)
        {
            //Turma turma = JsonConvert.DeserializeObject<Turma>(jsonTurma);

            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);

            //definição do comando sql
            string sql = "UPDATE Aluno SET nome =  @nome," +
                " cpf = @cpf, datanasc = @datanasc, telefone =  @telefone," +
                " email = @email, infoadic = @infoadic" +
                " WHERE id = @id";

            try
            {
                SqlCommand comando = new SqlCommand(sql, conn);

                comando.Parameters.Add(new SqlParameter("@id", aluno.Id));
                comando.Parameters.Add(new SqlParameter("@nome", aluno.Nome));
                comando.Parameters.Add(new SqlParameter("@cpf", aluno.CPF));
                comando.Parameters.Add(new SqlParameter("@datanasc", aluno.DataNasc));
                comando.Parameters.Add(new SqlParameter("@telefone", aluno.Telefone));
                comando.Parameters.Add(new SqlParameter("@email", aluno.Email));
                comando.Parameters.Add(new SqlParameter("@infoadic", aluno.InfoAdic));

                conn.Open();
                comando.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    Mensagem = "Erro Ao Atualizar aluno"
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Close();
            }

            return Json(new
            {
                Sucesso = true,
                Mensagem = "Alterado com Sucesso"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoverAluno(int Id, string NumTurma)
        {

            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);

            try
            {
               if (NumTurma == "")
                {
                    string sql = "DELETE FROM Matricula WHERE alunoid = @id";

                    SqlCommand comando = new SqlCommand(sql, conn);

                    comando.Parameters.Add(new SqlParameter("@id", Id));

                    conn.Open();
                    comando.ExecuteNonQuery();
                    conn.Close();

                    sql = "DELETE FROM Aluno WHERE id = @id";

                    comando = new SqlCommand(sql, conn);

                    comando.Parameters.Add(new SqlParameter("@id", Id));

                    conn.Open();
                    comando.ExecuteNonQuery();
                    conn.Close();

                }
                else
                {
                    string sql = "DELETE FROM Matricula WHERE turmaid = " +
                "(SELECT TOP(1) id FROM Turma WHERE numturma = @numturma) and alunoid = @id";

                    SqlCommand comando = new SqlCommand(sql, conn);

                    comando.Parameters.Add(new SqlParameter("@numturma", NumTurma));
                    comando.Parameters.Add(new SqlParameter("@id", Id));

                    conn.Open();
                    comando.ExecuteNonQuery();
                    conn.Close();

                }
                

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    Mensagem = "Erro Ao Remover aluno da turma"
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Close();
            }

            return Json(new
            {
                Sucesso = true,
                Mensagem = "Removido com Sucesso"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerProfessores(Professores professores)
        {
            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);
            //definição do comando sql

            string sql = "SELECT * " +
                " FROM dbo.Professor ";

            try
            {
                professores.Professor = new List<Professor>();

                SqlCommand comando = new SqlCommand(sql, conn);

                conn.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Professor p = new Professor();
                            p.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            p.Nome = reader.GetString(reader.GetOrdinal("nome"));
                            p.CPF = reader.GetString(reader.GetOrdinal("cpf"));
                            p.DataNasc = reader.GetDateTime(reader.GetOrdinal("datanasc"));
                            p.Telefone = reader.GetString(reader.GetOrdinal("telefone"));
                            professores.Professor.Add(p);
                        }
                    }
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                return Content("Erro ao visualizar os professores");
            }
            finally
            {
                conn.Close();
            }

            return View("Professores", professores);
        }

        public ActionResult ConsultaProfessor(string CodProfessor)
        {
            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);
            //definição do comando sql
            string sql = "SELECT nome, datanasc ,telefone, cpf " +
                         " FROM dbo.Professor WHERE id = " + CodProfessor;

            Professor Professor = new Professor();

            try
            {

                SqlCommand comando = new SqlCommand(sql, conn);

                conn.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Professor.Nome = Convert.ToString(reader["nome"]);
                            Professor.CPF = Convert.ToString(reader["cpf"]);
                            Professor.DataNasc = Convert.ToDateTime(reader["datanasc"]);
                            Professor.Telefone = Convert.ToString(reader["telefone"]);
                        }
                    }
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Close();
            }

            return Json(Professor, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AtualizarProfessor(Professor Professor)
        {

            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);

            //definição do comando sql
            string sql = "UPDATE Professor SET nome =  @nome," +
                " cpf = @cpf, datanasc = @datanasc, telefone =  @telefone" +
                " WHERE id = @id";

            try
            {
                SqlCommand comando = new SqlCommand(sql, conn);

                comando.Parameters.Add(new SqlParameter("@id", Professor.Id));
                comando.Parameters.Add(new SqlParameter("@nome", Professor.Nome));
                comando.Parameters.Add(new SqlParameter("@cpf", Professor.CPF));
                comando.Parameters.Add(new SqlParameter("@datanasc", Professor.DataNasc));
                comando.Parameters.Add(new SqlParameter("@telefone", Professor.Telefone));

                conn.Open();
                comando.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    Mensagem = "Erro Ao Atualizar Professor"
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Close();
            }

            return Json(new
            {
                Sucesso = true,
                Mensagem = "Alterado com Sucesso"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoverProfessor(int Id)
        {

            string cs = ConfigurationManager.ConnectionStrings["EscolaTECNUN"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);
            //definição do comando sql
            string sql = "SELECT * " +
                         " FROM dbo.Turma WHERE professorid = " + Id;

            SqlCommand comando = new SqlCommand(sql, conn);

            conn.Open();

            SqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)
            {
                conn.Close();

                return Json(new
                {
                    Sucesso = false,
                    Mensagem = "Existe turma com este professor para ministrar aula. Favor retirar este professor dessa(s) turma(s)."
                }, JsonRequestBehavior.AllowGet);
            }

            conn.Close();


            try
            {
                    sql = "DELETE FROM Professor WHERE id = @id";

                    comando = new SqlCommand(sql, conn);

                    comando.Parameters.Add(new SqlParameter("@id", Id));

                    conn.Open();
                    comando.ExecuteNonQuery();
                    conn.Close();

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    Mensagem = "Erro Ao Remover Professor"
                }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Close();
            }

            return Json(new
            {
                Sucesso = true,
                Mensagem = "Removido com Sucesso"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}