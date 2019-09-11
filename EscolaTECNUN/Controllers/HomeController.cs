using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using EscolaTECNUN.Models;
using System.Configuration;

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
            string sql = "SELECT a.id, a.nome " +
                " FROM dbo.Aluno a " +
                " INNER JOIN dbo.Matricula m ON m.alunoid = a.id" +
                " INNER JOIN dbo.Turma t ON t.id = m.turmaid" +
                " WHERE t.numturma = @numturma";

            try
            {
                infoturma.Aluno = new List<Aluno>();

                SqlCommand comando = new SqlCommand(sql, conn);

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

            return View("VerAlunos",infoturma);
        }
    }
}