$('#Login').on('click', function (eventObj) {
    var Logindata = {
        _id: $('#Loginid').val(),
        password: $('#LoginPW').val(),
    };
    $.ajax({
        method: 'POST',
        url: 'http://localhost:8888/api/Login',
        dataType: 'JSON',
        data: Logindata,
        success: function (data) {            
            if (data.ok == false) {
                alert(data.errmsg);
            }
            if (data.Identity == "管理") {
                window.location.href = 'http://localhost:8888/HTML/Manager.html';
                $.cookie('UserID', data._id);
                $.cookie('UserName', data.Name);
                $.cookie('token', data.token);                
            }
            if (data.Identity == "學生") {
                window.location.href = 'http://localhost:8888/HTML/Student.html';
                $.cookie('UserID', data._id);
                $.cookie('UserName', data.Name);
                $.cookie('token', data.token);
            }
            if (data.Identity == "教授") {
                window.location.href = 'http://localhost:8888/HTML/Professor.html';
                $.cookie('UserID', data._id);
                $.cookie('UserName', data.Name);
                $.cookie('token', data.token);
            }
        },
        error: function (data) { console.log('error: '); console.log(data); $('body').append(response); },
    });
});