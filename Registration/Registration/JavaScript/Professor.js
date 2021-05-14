$('#CourseBtn').on('click', function (eventObj) {
    $('#ShowBox').empty();    
    $("#AddCourseBtn").trigger('click');
    $('#ShowBox').append('<h2>所有課程</h2>')
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

$('#selfCourseBtn').on('click', function (eventObj) {
    $('#ShowBox').empty();   
    $("#DelCourseBtn").trigger('click');
    $('#ShowBox').append('<h2>自己課程</h2>')
    var showtable = $('<table class="table"></table>');
    var showtbody = $('<tbody></tbody>');
    showtable.append('<thead><tr><th border="1">課程編號</th><th>課程名稱</th><th>教授</th><th>課堂節數</th><th>選課人數</th><th>上限人數</th></tr></thead>');
    showtable.append(showtbody);
    $('#ShowBox').append(showtable);
    $.ajax({
        method: 'GET',
        url: 'http://localhost:8888/api/course/' + $.cookie('UserID'),
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

$('#AddCourseBtn').on('click', function (eventObj) {
    $('#functionBox').empty();
    var showBtn = $('<div></div>');
    showBtn.append('<h3>輸入資料</h3>');
    showBtn.append('<input id="Name" placeholder="課程名稱"/>');
    showBtn.append('<p>課程內容</p><textarea id=Description style=resize:none;width:600px;height:200px;></textarea>');
    showBtn.append('<p>課堂上限人數</p>')
    var selectbtn = $('<select id = number></select>');
    for (var i = 1; i <= 30; i++) {
        selectbtn.append('<option value='+ i + '>'+ i +'</option>')
    }    
    showBtn.append(selectbtn);
    showBtn.append('<p>課堂節數</p>')
    showBtn.append('<select id = Periodsnum><option value=1>1</option><option value=2>2</option><option value=3>3</option></select>');
    showBtn.append('<br><button id="AddBtn">送出</button>');
    $('#functionBox').append(showBtn);

    showBtn.find('#AddBtn').on('click', function (eventObj) {        
        var checksummit = confirm('確認送出?');
        if (checksummit) {
            var Coursedata = {                
                Name : $('#Name').val(),
                Description : $('#Description').val(),
                Professor : $.cookie('UserName'),
                ProfessorID : $.cookie('UserID'),
                Periods : $('#Periodsnum').val(),
                UpLimit : $('#number').val(),
            };
            if ($('#Name').val() === "" || $('#Description').val() === "") {
                return alert("請輸入完整資料")
            }
            $.ajax({
                method: 'POST',
                url: 'http://localhost:8888/api/course',
                dataType: 'JSON',
                data: Coursedata,
                success: function (data) {
                    if (data.ok == false) {
                        alert(data.errmsg);
                    }
                    if (data.ok == true) {
                        alert('建立成功')
                        $("#selfCourseBtn").trigger('click');
                    }
                },
            });
        };
        
    });
});    

$('#DelCourseBtn').on('click', function (e) {
    $('#functionBox').empty();
    var showBtn = $('<div></div>');
    showBtn.append('<h3>刪除課程</h3>');
    showBtn.append('<input id="ID" placeholder="輸入課程編號"/>');
    showBtn.append('<button id="DelBtn">送出</button>');
    $('#functionBox').append(showBtn);

    showBtn.find('#DelBtn').click(function (e) {
        var DelID = $('#ID').val();
        var ProfessorID = $.cookie('UserID')        

        $.ajax({
            method: "DELETE",
            url: 'http://localhost:8888/api/course/' + DelID + '/' + ProfessorID,
            dataType: 'JSON',
            success: function (data) {
                if (data.ok == false) {
                    alert(data.errmsg);
                }
                if (data.ok == true) {
                    alert('已刪除' + DelID);
                    $("#selfCourseBtn").trigger('click');
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

$("#CourseBtn").trigger('click');
