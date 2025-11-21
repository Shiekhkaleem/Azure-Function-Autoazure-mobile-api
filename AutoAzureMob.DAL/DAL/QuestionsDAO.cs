using AutoAzureMob.Models.DTO.QuestionsDTO;
using AutoAzureMob.Models.Models.Questions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.DAL.DAL
{
    public class QuestionsDAO   :BaseDAO
    {
        private readonly IConfiguration _config;
        public QuestionsDAO(ExecuteContext executeContext, IConfiguration config) : base(executeContext, config)
        {
            _config = config;
        }
        #region Get Questions List
        public List<Question> GetQuestionsByStatus(QuestionDTO questionSearch)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                #region Setting Login User values to DB param
                new SqlParameter("@UserID", questionSearch.UserMKTId),
                new SqlParameter("@Status", questionSearch.Status),
                new SqlParameter("@SearchBy", questionSearch.SearchBy),
                new SqlParameter("@SearchText", questionSearch.SearchText),
                new SqlParameter("@SortBy", questionSearch.SortBy),
                new SqlParameter("@Page", questionSearch.Page),

                #endregion
            };
            string queryName = "MOB_MKT_MELI_ListQuestions";
            List<Question> questions = FetchQuestions(param, queryName);
            return questions;
        }
        private List<Question> FetchQuestions(List<SqlParameter> sqlParam, string queryName)
        {
            List<Question> questions = null;
            DataSet resultSet = null;
            if (!string.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, sqlParam, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable questionTable = resultSet.Tables[0];
                    if (questionTable.Rows.Count > 0)
                    {
                        questions = new List<Question>();
                        foreach (DataRow row in questionTable.Rows)
                        {
                            Question question = new Question();

                            #region getting Questions list
                            question.QuestionID = row["QuestionID"].ToString();
                            question.FromID = row["FromID"].ToString();
                            question.FromName = row["FromName"].ToString();
                            question.Text = row["Text"].ToString();
                            question.ItemID = row["ItemID"].ToString();
                            question.DateCreated =!string.IsNullOrEmpty(row["DateCreated"].ToString()) ? Convert.ToDateTime(row["DateCreated"]).ToString("yyyy-MM-dd hh:mm tt"): string.Empty;
                            if (!string.IsNullOrEmpty(question.DateCreated))
                            {
                                DateTime date = Convert.ToDateTime(question.DateCreated);
                                DateTime today = DateTime.Now;
                                TimeSpan span = today - date;
								question.TotalDays = span switch
								{
									{ Days: < 1 } => span.Hours switch 
                                    { 
                                        <= 1 => $"{span.Minutes} min",
                                        > 1 => $"{span.Hours} hour{span.Hours switch { > 1 => "s", _ => string.Empty }}, {span.Minutes} min"
									},
                                    { Days : >=1} => $"{span.Days} day{span.Days switch { > 1 => "s", _ => string.Empty }}"
								};
                            }
                            question.AnswerDateCreated = row["AnswerDateCreated"] != DBNull.Value ? row["AnswerDateCreated"].ToString() : "";
                            question.AnswerText = row["AnswerText"] != DBNull.Value ? row["AnswerText"].ToString() : "";
                            question.AvailableQuantity = row["AvailableQuantity"].ToString() ?? string.Empty;
                            question.Title = row["Title"].ToString() ?? string.Empty;
                            question.Price = row["Price"].ToString() ?? string.Empty;
                            question.TotalRows = Convert.ToInt32(row["TotalRows"].ToString()?? default);
                            question.Thumbnail = row["Thumbnail"] != DBNull.Value ? row["Thumbnail"].ToString() : "";
                            if (questionTable.Columns.Contains("Permalink"))
                            {
                                question.Permalink = row["Permalink"].ToString() ?? string.Empty;
                            }
                            if (questionTable.Columns.Contains("Status"))
                            {
                                question.Status = row["Status"].ToString() ?? string.Empty;
                            }
                            if (questionTable.Columns.Contains("OfficialStoreName"))
                            {
                                question.OfficialStoreName = row["OfficialStoreName"].ToString() ?? string.Empty;
                            }
                            if (questionTable.Columns.Contains("LogisticType"))
                            {
                                question.LogisticType = row["LogisticType"].ToString() ?? string.Empty;
                            }
                            question.SKU = row["SKU"].ToString()?? string.Empty;
                            CultureInfo cultureInfo = new CultureInfo("en-US");
                            TextInfo textInfo = cultureInfo.TextInfo;
                            question.ShippingMode = textInfo.ToUpper(row["ShippingMode"].ToString())?? string.Empty;
                            #endregion
                            questions.Add(question);
                        }
                    }
                }
            }
            return questions;
        }
        #endregion
        #region Get Question Details By Id
        public Question GetQuestionDetailById(QAHistoryDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                #region Setting Login User values to DB param
                new SqlParameter("@UserID", req.UserMKTId),
                new SqlParameter("@QuestionID", req.QuestionId)

                #endregion
            };
            string queryName = "MLK_MELI_GetQuestionByID";
            Question questions = FetchQuestionDetailById(param, queryName);
            return questions;
        }
        private Question FetchQuestionDetailById(List<SqlParameter> sqlParam, string queryName)
        {
            Question question = null;
            DataSet resultSet = null;
            if (!string.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, sqlParam, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable questionTable = resultSet.Tables[0];
                    if (questionTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in questionTable.Rows)
                        {
                            question = new Question();

                            #region getting Questions list
                            question.QuestionID = row["QuestionID"].ToString();
                            question.FromID = row["FromID"].ToString();
                            question.FromName = row["FromName"].ToString();
                            question.Text = row["Text"].ToString();
                            question.ItemID = row["ItemID"].ToString();
                            question.DateCreated = row["DateCreated"].ToString();
                            if (!string.IsNullOrEmpty(question.DateCreated))
                            {
                                DateTime date = Convert.ToDateTime(question.DateCreated);
                                DateTime today = DateTime.Now;
                                TimeSpan span = today - date;
                                question.TotalDays = Math.Round((decimal)span.TotalDays).ToString() + "D, " + Math.Round((decimal)span.Hours).ToString() + ":" + Math.Round((decimal)span.Minutes).ToString() + "H";
                            }
                            question.AnswerDateCreated = row["AnswerDateCreated"] != DBNull.Value ? row["AnswerDateCreated"].ToString() : "";
                            question.AnswerText = row["AnswerText"] != DBNull.Value ? row["AnswerText"].ToString() : "";
                            question.AvailableQuantity = row["AvailableQuantity"].ToString();
                            question.Title = row["Title"].ToString();
                            question.Price = row["Price"].ToString();
                            question.TotalRows = Convert.ToInt32(row["TotalRows"].ToString());
                            question.Thumbnail = row["Thumbnail"] != DBNull.Value ? row["Thumbnail"].ToString() : "";
                            if (questionTable.Columns.Contains("Permalink"))
                            {
                                question.Permalink = row["Permalink"].ToString();
                            }
                            if (questionTable.Columns.Contains("Status"))
                            {
                                question.Status = row["Status"].ToString();
                            }
                            if (questionTable.Columns.Contains("OfficialStoreName"))
                            {
                                question.OfficialStoreName = row["OfficialStoreName"].ToString();
                            }
                            if (questionTable.Columns.Contains("LogisticType"))
                            {
                                question.LogisticType = row["LogisticType"].ToString();
                            }
                            question.SKU = row["SKU"].ToString();
                            CultureInfo cultureInfo = new CultureInfo("en-US");
                            TextInfo textInfo = cultureInfo.TextInfo;
                            question.ShippingMode = textInfo.ToUpper(row["ShippingMode"].ToString()) ?? string.Empty;
                            #endregion
                        }
                    }
                }
            }
            return question;
        }
        #endregion
        #region Questions and Answers History
        public List<QuestionHistory> GetQuestionAnswerHistory(QAHistoryDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                #region Setting Login User values to DB param
                new SqlParameter("@QuestionID", req.QuestionId),
                new SqlParameter("@UserID", req.UserMKTId)
                #endregion
            };
            string queryName = "MOB_MKT_MELI_ListQuestionHistory";
            List<QuestionHistory> questionsHistory = FetchQuestionsAnswerHistory(param, queryName);
            return questionsHistory;
        }
        private List<QuestionHistory> FetchQuestionsAnswerHistory(List<SqlParameter> sqlParam, string queryName)
        {
            List<QuestionHistory> questionHistories = null;
            DataSet resultSet = null;
            if (!string.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, sqlParam, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable questionTable = resultSet.Tables[0];
                    if (questionTable.Rows.Count > 0)
                    {
                        questionHistories = new List<QuestionHistory>();
                        foreach (DataRow row in questionTable.Rows)
                        {
                            QuestionHistory questionHistory = new QuestionHistory();

                            #region getting Questions list
                            questionHistory.ID = row["ID"].ToString()?? "";
                            questionHistory.Message = row["QuestionText"].ToString()?? "";
                            questionHistory.Date = row["QuestionDate"].ToString()?? "";
                            questionHistory.UserMKTId =Convert.ToInt32( row["UserID"]?? 0);
                            #endregion
                            questionHistories.Add(questionHistory);
                        }
                    }
                }
            }
            return questionHistories;
        }
        #endregion
        #region Quick Answers List
        public List<QuickAnswer> GetQuickAnswersList(string userId)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@UserID", userId),
            };
            string queryName = "MOB_MKT_MELI_ListQuickAnswers";
            List<QuickAnswer> quickAnswers = FetchQuickAnswers(param, queryName);
            return quickAnswers;
        }
        private List<QuickAnswer> FetchQuickAnswers(List<SqlParameter> sqlParam, string queryName)
        {
            List<QuickAnswer> quickAnswers = null;
            DataSet resultSet = null;
            if (!string.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, sqlParam, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable answersTable = resultSet.Tables[0];
                    if (answersTable.Rows.Count > 0)
                    {
                        quickAnswers = new List<QuickAnswer>();
                        foreach (DataRow row in answersTable.Rows)
                        {
                            QuickAnswer quickAnswer = new QuickAnswer();
                            quickAnswer.ID = row["ID"].ToString();
                            quickAnswer.Name = row["Name"].ToString();
                            quickAnswer.Text = row["Text"].ToString();
                            quickAnswer.DateCreated = row["DateCreated"].ToString();
                            quickAnswers.Add(quickAnswer);
                        }
                    }
                }
            }
            return quickAnswers;
        }
        #endregion
    }
}
