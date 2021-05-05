using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Registration.Models;
using MongoDB.Bson.Serialization;

namespace Registration.Controllers
{
    public class RegistrationController : ApiController
    {
        //登入
        [Route("api/Login")]
        [HttpPost]
        public GetLoginResponse Post(Login request)
        {
            var response = new GetLoginResponse();
            var client = new MongoClient("mongodb://localhost:27017");
            MongoDatabaseBase db = client.GetDatabase("ClassProject") as MongoDatabaseBase;
            var AccountCol = db.GetCollection<MemberCollection>("Members");
            var AccountFilter = Builders<MemberCollection>.Filter;

            var EqID = AccountFilter.Eq(e=>e._id, request._id);
            var findpw = AccountCol.Find(EqID).Project(new BsonDocument { { "Password",true} }).ToListAsync().Result;

            /*random token*/
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            int passwordLength = 6;//密碼長度
            char[] chars = new char[passwordLength];
            Random rd = new Random();
            for (int i = 0; i < passwordLength; i++)
            {
                //allowedChars -> 這個String ，隨機取得一個字，丟給chars[i]
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
                    if( request.password != doc.Password)
                    {
                        response.ok = false;
                        response.errmsg = "密碼錯誤";
                    }
                    else
                    {
                        response._id = doc._id;
                        response.token = token; 
                    }
                }
            }
            return response;
        }
        
        //建立成員資料
        [Route("api/member")]
        [HttpPost]
        public SingnupResponser Post(SignupMember request)
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
                    States = request.States,
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

            var AccountCol = db.GetCollection<MemberCollection>("Members");
            var AllInfo = AccountCol.Find(new BsonDocument { }).ToListAsync().Result;

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
                        States = doc.States
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
            var DelID = Membercol.DeleteOne(EqID);

            if (DelID.DeletedCount == 0)
            {
                response.ok = false;
                response.errmsg = "無此會員，無法刪除";
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
            if(FindID != null)
            {
                response.ok = false;
                response.errmsg = "此編號" + request._id + "已建立";
            }
            else
            {
                Coursecol.InsertOne(new CourseCollection()
                {
                    _id = request._id,
                    Name = request.Name,
                    Description = request.Description,
                    Professor = request.Professor,
                    Periods = request.Periods,                    
                    Uplimit = request.Uplimit,
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
            var FindCourse = Coursecol.Find(new BsonDocument { }).ToListAsync().Result;
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
                        Uplimit = doc.Uplimit,
                    });
            }
            return response;
        }

        //刪除課程
        [Route("api/course")]



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
