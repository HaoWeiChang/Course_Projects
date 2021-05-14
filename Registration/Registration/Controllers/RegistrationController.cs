using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using Registration.Models;


namespace Registration.Controllers
{
    public class RegistrationController : ApiController
    { 
        //登入
        [Route("api/Login")]
        [HttpPost]
        public GetLoginResponse Login(LoginMember request)
        {            
            var response = new GetLoginResponse();
            if (request._id == null || request.password == null)
            {
                response.ok = false;
                response.errmsg = "帳號或密碼不能為空值";
                return response;
            };
            var client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;
            var AccountCol = db.GetCollection<MemberCollection>("Members");
            var CookieCol = db.GetCollection<Cookies>("Cookies");

            var AccountFilter = Builders<MemberCollection>.Filter;            
            var EqID = AccountFilter.Eq(e=>e._id, request._id);
            var findpw = AccountCol.Find(EqID).Project(new BsonDocument { { "Password",true},{ "Identity",true},{ "Name",true} }).ToListAsync().Result;
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            int passwordLength = 10;
            char[] chars = new char[passwordLength];
            Random rd = new Random();
            for (int i = 0; i < passwordLength; i++)
            {
                
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            string token = new string(chars);            
            if (findpw.Count() == 0)
            {
                response.ok = false;
                response.errmsg = "無此學號或教師號碼";
            }
            else
            {
                foreach (BsonDocument Bsondoc in findpw)
                {
                    var doc = BsonSerializer.Deserialize<MemberCollection>(Bsondoc);
                    if (request.password != doc.Password)
                    {
                        response.ok = false;
                        response.errmsg = "密碼錯誤";
                    }
                    else
                    {
                        CookieCol.InsertOne(new Cookies { UserID = doc._id, Usertoken = token });
                        response._id = doc._id;
                        response.token = token;
                        response.Identity = doc.Identity;
                        response.Name = doc.Name;
                    }
                }
            }
            return response;
        }

         //建立成員資料
        [Route("api/member")]
        [HttpPost]
        public SingnupResponser AddMember(SignupMember request)
        {
            var response = new SingnupResponser();
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;

            var AccountCol = db.GetCollection<MemberCollection>("Members");
            var AccountFilter = Builders<MemberCollection>.Filter;

            var eqid = AccountFilter.Eq(e => e._id, request._id);
            var eqemail = AccountFilter.Eq(e => e.Email, request.Email);
            var IDFindResult = AccountCol.Find(eqid).ToListAsync().Result.FirstOrDefault();
            var EmailFindResult = AccountCol.Find(eqemail).ToListAsync().Result.FirstOrDefault();

            if (IDFindResult != null)
            {
                response.ok = false;
                response.errmsg = "此學號已有資料";
                return response;
            }
            if (EmailFindResult != null)
            {
                response.ok = false;
                response.errmsg = "含有相同Email請重新填寫";
                return response;
            }
            if (IDFindResult == null && EmailFindResult == null)
            {
                AccountCol.InsertOne(new MemberCollection()
                {
                    _id = request._id,
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,
                    Faculty = request.Faculty,
                    Identity = request.Identity,                    
                });
            }
            return response;
        }
        
        //取得成員清單
        [Route("api/member")]
        [HttpGet]
        public GetMemberListResponse GetMember()
        {
            var response = new GetMemberListResponse();
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;
            var MemberSort = Builders<MemberCollection>.Sort;
            var MemberCol = db.GetCollection<MemberCollection>("Members");
            var sort = MemberSort.Ascending(e => e.Identity);
            var AllInfo = MemberCol.Find(new BsonDocument { }).Sort(sort).ToListAsync().Result;

            foreach (var doc in AllInfo)
            {
                response.MembersList.Add(
                    new Members()
                    {
                        _id = doc._id,
                        Name = doc.Name,
                        Email = doc.Email,
                        Password = doc.Password,
                        Faculty = doc.Faculty,
                        Identity = doc.Identity,                        
                    });
            }
            return response;
        }
        
        //刪除成員
        [Route("api/member/{ID}")]
        [HttpDelete]
        public DelMemberResponse Del(string id)
        {
            var response = new DelMemberResponse();
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;
            
            var Membercol = db.GetCollection<MemberCollection>("Members");
            var Memberfilter = Builders<MemberCollection>.Filter;
            var EqID = Memberfilter.Eq(e => e._id, id);
            var FindID = Membercol.Find(EqID).ToListAsync().Result;            
            if (id == "main01")
            {
                response.ok = false;
                response.errmsg = "無法刪除管理者";
            }
            else if (FindID.Count() == 0)
            {
                response.ok = false;
                response.errmsg = "無此會員，無法刪除";
            }
            else
            {
                Membercol.DeleteOne(EqID);
            }
            return response;
        }

        //建立課程
        [Route("api/course")]
        [HttpPost]
        public AddCourseResponse PostCourse(AddCourse request)
        {
            var response = new AddCourseResponse();
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;
            var Coursecol = db.GetCollection<CourseCollection>("Course");
            var CourseFilter = Builders<CourseCollection>.Filter;            
            var EqID = CourseFilter.Eq(e => e._id, request._id);
            var FindID = Coursecol.Find(EqID).ToListAsync().Result.FirstOrDefault();
            string allowedChars = "0123456789";
            int numLength = 6;
            char[] chars = new char[numLength];
            Random rd = new Random();
            for (int i = 0; i < numLength; i++)
            {

                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            string token = new string(chars);

            if (FindID != null)
            {
                response.ok = false;
                response.errmsg = "此編號" + request._id + "已建立";
            }
            else
            {
                Coursecol.InsertOne(new CourseCollection()
                {
                    _id = token,
                    Name = request.Name,
                    Description = request.Description,
                    Professor = request.Professor,
                    ProfessorID = request.ProfessorID,
                    Periods = request.Periods,
                    Uplimit = request.Uplimit,
                    Count = 0,
                    ChooseList = new[] { new ChooseMember { } }
                }); 
            }
            return response;
        }

        //取得所有課程清單
        [Route("api/course")]
        [HttpGet]
        public GetCourseResponse GetCourseLits()
        {
            var response = new GetCourseResponse();
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;
            var Coursecol = db.GetCollection<CourseCollection>("Course");
            var Coursesort = Builders<CourseCollection>.Sort;            
            var sort = Coursesort.Ascending(e => e._id);
            var FindCourse = Coursecol.Find(new BsonDocument { }).Sort(sort).ToListAsync().Result;
            foreach(CourseCollection doc in FindCourse)
            {
                response.CoursesList.Add(
                    new Courses()
                    {
                        _id = doc._id,
                        Name = doc.Name,
                        Description = doc.Description,
                        Professor = doc.Professor,
                        Periods = doc.Periods,
                        Count = doc.Count,
                        Uplimit = doc.Uplimit,
                    });
            }
            return response;
        }

        //取得本人課程
        [Route("api/course/{ID}")]
        [HttpGet]
        public GetCourseResponse GetselfCourse(string ID)
        {
            var response = new GetCourseResponse();
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;
            var Coursecol = db.GetCollection<CourseCollection>("Course");
            var CourseFilter = Builders<CourseCollection>.Filter;
            var Coursesort = Builders<CourseCollection>.Sort;
            var sort = Coursesort.Ascending(e => e._id);
            var EqName = CourseFilter.Eq(e => e.ProfessorID, ID);
            var FindCourse = Coursecol.Find(EqName).Sort(sort).ToListAsync().Result;
            foreach (CourseCollection doc in FindCourse)
            {
                response.CoursesList.Add(
                    new Courses()
                    {
                        _id = doc._id,
                        Name = doc.Name,
                        Description = doc.Description,
                        Professor = doc.Professor,
                        Periods = doc.Periods,
                        Count = doc.Count,
                        Uplimit = doc.Uplimit,
                    });
            }
            return response;
        }

        //刪除課程
        [Route("api/course/{ID}/{UserID}")]
        [HttpDelete]
        public DelCourseResponse DelCourse(string id,string UserID)
        {
            var response = new DelCourseResponse();
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;

            var Coursecol = db.GetCollection<CourseCollection>("Course");
            var Coursefilter = Builders<CourseCollection>.Filter;
            var EqID = Coursefilter.Eq(e => e._id, id);
            var CheckID = Coursecol.Find(EqID).ToListAsync().Result;            
            if (CheckID.Count() == 0)
            {
                response.ok = false;
                response.errmsg = "無此課程，無法刪除";
            }
            foreach (CourseCollection doc in CheckID)
            {
                if (doc.ProfessorID == UserID || UserID == "main01")
                {
                   Coursecol.DeleteOne(EqID);
                }                
                else
                {
                    response.ok = false;
                    response.errmsg = "非本人課程，無法刪除";
                }
            }   
            return response;
        }
    
        //加選課程
        [Route("api/Choosecourse")]
        [HttpPost]
        public ChooseCourseResponse PostChooseCourse(ChooseCourse request)
        {
            var response = new ChooseCourseResponse();
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;

            var Coursecol = db.GetCollection<CourseCollection>("Course");
            var CourseFilter = Builders<CourseCollection>.Filter;
            var ChooseFilter = Builders<ChooseMember>.Filter;
            var CourseUpdate = Builders<CourseCollection>.Update;            
            var EqCourseID = CourseFilter.Eq(e => e._id,request.CourseID);
            var FindCourse = Coursecol.Find(EqCourseID).ToListAsync().Result;
            
            var EqChooseID = ChooseFilter.Eq(e => e.student_ID, request.UserID);
            var elematch = CourseFilter.ElemMatch(e => e.ChooseList, EqChooseID);
            var mixEq = CourseFilter.And(EqCourseID, elematch);
            var CheckUserID = Coursecol.Find(mixEq).ToListAsync().Result;

            if (FindCourse.Count() == 0)
            {
                response.ok = false;
                response.errmsg = "無此課程編號";
                return response;
            }
            if (CheckUserID.Count() != 0)
            {
                response.ok = false;
                response.errmsg = "您已選擇此課程";
                return response;
            }
            if (FindCourse[0].Uplimit == FindCourse[0].Count.ToString())
            {
                response.ok = false;
                response.errmsg = "人數已滿";
                return response;
            }
            else
            {
                var Countadd = CourseUpdate.Inc(e => e.Count, 1);
                Coursecol.UpdateOne(EqCourseID, Countadd);

                var ChooseUser = new ChooseMember()
                {
                    student_Name = request.UserName,
                    student_ID = request.UserID,
                };

                var Adddata = CourseUpdate.AddToSet(e => e.ChooseList, ChooseUser);
                Coursecol.UpdateOne(EqCourseID, Adddata);
            }  
            return response;
        }

        //退選課程
        [Route("api/Choosecourse")]
        [HttpDelete]
        public DelectChooseResponse DelChoose(ChooseCourse request)
        {
            var response = new DelectChooseResponse();
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;

            var Coursecol = db.GetCollection<CourseCollection>("Course");
            var CourseFilter = Builders<CourseCollection>.Filter;
            var ChooseFilter = Builders<ChooseMember>.Filter;
            var CourseUpdate = Builders<CourseCollection>.Update;

            var EqCourseID = CourseFilter.Eq(e => e._id, request.CourseID);       
            var EqChooseID = ChooseFilter.Eq(e => e.student_ID, request.UserID);
            var elematch = CourseFilter.ElemMatch(e => e.ChooseList, EqChooseID);
            var mixEq = CourseFilter.And(EqCourseID, elematch);
            var CheckUserID = Coursecol.Find(mixEq).ToListAsync().Result;
            var FindCourse = Coursecol.Find(EqCourseID).ToListAsync().Result;

            if (FindCourse.Count() == 0) {
                response.ok = false;
                response.errmsg = "無此課程編號";
                return response;
            }

            if (CheckUserID.Count() == 0)
            {
                response.ok = false ;
                response.errmsg = "你未選擇此課程";
            }
            else
            {
                var Countadd = CourseUpdate.Inc(e => e.Count, -1);
                Coursecol.UpdateOne(EqCourseID, Countadd);

                var ChooseUser = new ChooseMember()
                {
                    student_Name = request.UserName,
                    student_ID = request.UserID,
                };

                var pulldata = CourseUpdate.Pull(e => e.ChooseList, ChooseUser);
                Coursecol.UpdateOne(EqCourseID, pulldata);
                
            }
            return response;
        }

        //已選擇課程
        [Route("api/Choosecourse/{ID}")]
        [HttpGet]
        public GetCourseResponse GetChooseList(string ID)
        {
            var response = new GetCourseResponse();            
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;

            var Coursecol = db.GetCollection<CourseCollection>("Course");
            var CourseFilter = Builders<CourseCollection>.Filter;
            var ChooseFilter = Builders<ChooseMember>.Filter;
            var CourseUpdate = Builders<CourseCollection>.Update;
            var Coursesort = Builders<CourseCollection>.Sort;
            var sort = Coursesort.Ascending(e => e._id);
            var EqChooseID = ChooseFilter.Eq(e => e.student_ID, ID);
            var elematch = CourseFilter.ElemMatch(e => e.ChooseList, EqChooseID);
            var CheckUserID = Coursecol.Find(elematch).Sort(sort).ToListAsync().Result;
            if(CheckUserID.Count() == 0)
            {
                response.ok = false;
                response.errmsg = "您未加選課程";
                return response;
            }

            foreach (CourseCollection doc in CheckUserID)
            {
                response.CoursesList.Add(
                    new Courses()
                    {
                        _id = doc._id,
                        Name = doc.Name,
                        Description = doc.Description,
                        Professor = doc.Professor,
                        Periods = doc.Periods,
                        Count = doc.Count,
                        Uplimit = doc.Uplimit,
                    });
            }
            return response;
        }

        //狀態檢查
        [Route("")]
        [HttpGet]        
        public HttpResponseMessage GetHealth()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new StringContent("Test API is Running");
            return response;
        }
    }
}
