$('#MemberBtn').on('click', function (eventObj) {    
    $('#ShowBox').empty();    
    $("#SignUpMenberBtn").trigger('click');
    var showtable = $('<table class="table"></table>');
    var showtbody = $('<tbody></tbody>');
    showtable.append('<thead><tr><th>學號</th><th>姓名</th><th>Email</th><th>密碼</th><th>科系</th><th>身分</th></tr></thead>');
    showtable.append(showtbody);
    $('#ShowBox').append(showtable);
    $.ajax({
        method: 'GET',
        url: 'http://localhost:8888/api/member',
        dataType: 'JSON',
        success: function (data) {
            var ListLength = data.MembersList.length
            for (var i = 0; i < ListLength; i++) {                
                var puttr = $('<tr></tr>');
                puttr.append('<th>' + data.MembersList[i]._id + '</th>' );
                puttr.append('<th>' + data.MembersList[i].Name + '</th>');      
                puttr.append('<th>' + data.MembersList[i].Email + '</th>');
                puttr.append('<th>' + data.MembersList[i].Password + '</th>');   
                puttr.append('<th>' + data.MembersList[i].Faculty + '</th>');
                puttr.append('<th>' + data.MembersList[i].Identity + '</th>');   
                showtbody.append(puttr);
            };  
        },
        error: function (data) { console.log('error: '); console.log(data); },
    });
});

$('#SignUpMenberBtn').on('click', function (eventObj) {
    $('#functionBox').empty();
    var showBtn = $('<div></div>');
    showBtn.append('<h3>新建資料</h3>');
    showBtn.append('<input id="ID" placeholder="輸入學號" /> <input id="Name" placeholder="輸入姓名" /> <input id="Password" placeholder="輸入密碼" />');
    showBtn.append('<div><h5>選擇系</h5> <input type="radio" name="Faculty" value="電子系" id="Faculty" checked/>電子系</div >');
    showBtn.append('<div><h5>選擇身分</h5><input type="radio" name="Identity" value="學生" id="Identity" checked/>學生<input type = "radio" name = "Identity" value = "教授" id = "Identity"/>教授</div><button id="SingupBtn">送出</button>');
    $('#functionBox').append(showBtn);
    showBtn.find('#SingupBtn').on('click', function (eventObj) {
        var Identitycheck = $('input[name=Identity]:checked');
        var Facultycheck = $('input[name=Faculty]:checked');
        var Memberdata = {
            Name: $('#Name').val(),
            _id: $('#ID').val(),
            Email: $('#ID').val() + "@gmail.com",
            Password: $('#Password').val(),
            Faculty: Facultycheck.val(),
            Identity: Identitycheck.val(),
        };
        if ($('#Name').val() === "" || $('#ID').val() === "" || $('#Password').val() === "" || Identitycheck === null || Facultycheck === null) {
            return alert("請輸入完整資料")
        }
        $.ajax({
            method: 'POST',
            url: 'http://localhost:8888/api/member',
            dataType: 'JSON',
            data: Memberdata,
            success: function (data) {
                if (data.ok == false) {
                    alert(data.errmsg);
                }
                if (data.ok == true) {
                    alert('建立成功')
                    $("#MemberBtn").trigger('click');
                }
            },
        });
    });


});    

$('#DelMemberBtn').on('click', function (e) {
    $('#functionBox').empty();
    var showBtn = $('<div></div>');
    showBtn.append('<h3>刪除成員</h3>');
    showBtn.append('<input id="ID" placeholder="輸入學號" />');
    showBtn.append('<button id="DelBtn">送出</button>');
    $('#functionBox').append(showBtn);

    showBtn.find('#DelBtn').click(function (e) {
        var DelID = $('#ID').val();
        console.log(DelID);
        $.ajax({
            method: "DELETE",
            url: 'http://localhost:8888/api/member/' + DelID,
            dataType: 'JSON',            
            success: function (data) {
                if (data.ok == false) {
                    alert(data.errmsg);
                }
                if (data.ok == true) {
                    alert('已刪除' + DelID);
                    $("#MemberBtn").trigger('click');
                }
            },            
        })
    });
    
});

$('#CourseBtn').on('click', function (eventObj) {
    $('#ShowBox').empty();  
    $("#DelCourseBtn").trigger('click');
    var showtable = $('<table class="table"></table>');
    var showtbody = $('<tbody></tbody>');
    showtable.append('<thead><tr><th border="1">課程編號</th><th>課程名稱</th><th>教授</th><th>課堂節數</th><th>選課人數</th><th>上限人數</th></tr></thead>');
    showtable.append(showtbody);
    $('#ShowBox').append(showtable);
    $.ajax({
        method: 'GET',
        url: 'http://localhost:8888/api/course',
        dataType: 'JSON',
        success: function (data) {
            var ListLength = data.CoursesList.length
            for (var i = 0; i < ListLength; i++) {
                var puttr = $('<tr></tr>');
                puttr.append('<th>' + data.CoursesList[i]._id + '</th>');
                puttr.append('<th>' + data.CoursesList[i].Name + '</th>');                
                puttr.append('<th>' + data.CoursesList[i].Professor + '</th>');
                puttr.append('<th>' + data.CoursesList[i].Periods + '</th>');
                puttr.append('<th>' + data.CoursesList[i].Count + '</th>');
                puttr.append('<th>' + data.CoursesList[i].Uplimit + '</th>');
                showtbody.append(puttr);
            };
        },
        error: function (data) { console.log('error: '); console.log(data); },
    });
});

$('#DelCourseBtn').on('click', function (e) {    
    $('#functionBox').empty();
    var showBtn = $('<div></div>');
    showBtn.append('<h3>刪除課程</h3>');
    showBtn.append('<input id="ID" placeholder="輸入課程編號" />');
    showBtn.append('<button id="DelBtn">送出</button>');
    $('#functionBox').append(showBtn);

    showBtn.find('#DelBtn').click(function (e) {
        var DelID = $('#ID').val();
        var ProfessorName = $.cookie('UserName')
        $.ajax({
            method: "DELETE",
            url: 'http://localhost:8888/api/course/' + DelID + '/' + ProfessorName,
            dataType: 'JSON',
            success: function (data) {
                if (data.ok == false) {
                    alert(data.errmsg);
                }
                if (data.ok == true) {
                    alert('已刪除' + DelID);
                    $("#CourseBtn").trigger('click');
                }
            },
        })
    });

});

$('#Signout').on('click', function (e) {
    $.removeCookie('UserID');
    $.removeCookie('UserName');
    $.removeCookie('token');
    window.location.href = 'http://localhost:8888/HTML/Login.html';
});

$("#MemberBtn").trigger('click');