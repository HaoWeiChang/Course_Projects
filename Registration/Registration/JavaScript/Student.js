$('#CourseBtn').on('click', function (eventObj) {
    $('#ShowBox').empty();    
    $("#ChooseBtn").trigger('click');
    $('#ShowBox').append('<h2>所有課程</h2>')
    var showtable = $('<table class="table"></table>');
    var showtbody = $('<tbody></tbody>');
    showtable.append('<thead><tr><th>課程編號</th><th>課程名稱</th><th>教授</th><th>課堂節數</th><th>選課人數</th><th>上限人數</th></tr></thead>');
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

$('#showchooseBtn').on('click', function (eventObj) {
    $('#ShowBox').empty();    
    $("#DelChooseBtn").trigger('click'); 
    $('#ShowBox').append('<h2>已選擇課程</h2>')
    var showtable = $('<table class="table"></table>');
    var showtbody = $('<tbody></tbody>');
    showtable.append('<thead><tr><th>課程編號</th><th>課程名稱</th><th>教授</th><th>課堂節數</th><th>選課人數</th><th>上限人數</th></tr></thead>');
    showtable.append(showtbody);
    $('#ShowBox').append(showtable);
    $.ajax({
        method: 'GET',
        url: 'http://localhost:8888/api/Choosecourse/'+$.cookie('UserID'),
        dataType: 'JSON',
        success: function (data) {            
            var ListLength = data.CoursesList.length;
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

$('#ChooseBtn').on('click', function (e) {
    $('#functionBox').empty();
    var showBtn = $('<div></div>');
    showBtn.append('<h3>加選課程</h3>');
    showBtn.append('<input id="ID" placeholder="輸入課程編號"/>');
    showBtn.append('<button id="AddBtn">送出</button>');
    $('#functionBox').append(showBtn);

    showBtn.find('#AddBtn').click(function (e) {             
        var ChooseData = {
            CourseID: $('#ID').val(),
            UserName: $.cookie('UserName'),
            UserID: $.cookie('UserID'),
        };        
        $.ajax({
            method: "POST",
            url: 'http://localhost:8888/api/Choosecourse/',
            dataType: 'JSON',
            data: ChooseData,
            success: function (data) {
                if (data.ok == false) {
                    alert(data.errmsg);
                }
                if (data.ok == true) {
                    alert(ChooseData.CourseID + '加選成功');
                    $("#showchooseBtn").trigger('click');
                }
            },
        });
    });
});

$('#DelChooseBtn').on('click', function (e) {
    $('#functionBox').empty();
    var showBtn = $('<div></div>');
    showBtn.append('<h3>退選課程</h3>');
    showBtn.append('<input id="ID" placeholder="輸入課程編號"/>');
    showBtn.append('<button id="DelBtn">送出</button>');
    $('#functionBox').append(showBtn);

    showBtn.find('#DelBtn').click(function (e) {        
        var ChooseData = {
            CourseID: $('#ID').val(),
            UserName: $.cookie('UserName'),
            UserID: $.cookie('UserID'),
        };
        $.ajax({
            method: "DELETE",
            url: 'http://localhost:8888/api/Choosecourse/',
            dataType: 'JSON',
            data: ChooseData,
            success: function (data) {
                if (data.ok == false) {
                    alert(data.errmsg);
                }
                if (data.ok == true) {
                    alert(ChooseData.CourseID + '退選成功');
                    $("#showchooseBtn").trigger('click');
                }
            },
        });
    });
});

$('#Signout').on('click', function (e) {
    $.removeCookie('UserID');
    $.removeCookie('UserName');
    $.removeCookie('token');
    window.location.href = 'http://localhost:8888/HTML/Login.html';
});

$("#CourseBtn").trigger('click');