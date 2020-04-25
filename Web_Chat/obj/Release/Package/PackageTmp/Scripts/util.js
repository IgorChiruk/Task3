$(function () {

    $('#chatBody').show();
    // Ссылка на автоматически-сгенерированный прокси хаба
    var chat = $.connection.chatHub;
    // Объявление функции, которая хаб вызывает при получении сообщений
    chat.client.addMessage = function (name, message, userid) {
        // Добавление сообщений на веб-страницу 
        $('#chatroom').append('<p><b id="UserName" >' + htmlEncode(name)+'</b>: ' + htmlEncode(message) + '</p>');
    };

    $('#message').css("width")

    chat.client.addMyMessage = function (name, message, userid) {
        // Добавление сообщений на веб-страницу 
        $('#chatroom').append('<p id="my"><b id="UserName" >' + htmlEncode(name) + '</b>: ' + htmlEncode(message) + '</p>');            
    };

    chat.client.addOtherMessage = function (name, message, userid) {
        // Добавление сообщений на веб-страницу 
        $('#chatroom').append('<p id="other" ><b id="UserName" >' + htmlEncode(name) + '</b>: ' + htmlEncode(message) + '</p>');       
    };

    $("#message").focus();

    

    // Функция, вызываемая при подключении нового пользователя
    chat.client.onConnected = function (id, userName, userId, allUsers) {       
        // установка в скрытых полях имени и id текущего пользователя
        $('#hdId').val(id);
        $('#username').val(userName);
        $('#UserId').val(userId)
        $('#header').html('<h3>Hello, ' + userName +'</h3>');
        chat.server.connect(userId);
        // Добавление всех пользователей
        for (i = 0; i < allUsers.length; i++) {

            AddUser(allUsers[i].ConnectionId, allUsers[i].Name);
        }
    }

    // Добавляем нового пользователя
    chat.client.onNewUserConnected = function (id, name) {

        AddUser(id, name);
    }

    // Удаляем пользователя
    chat.client.onUserDisconnected = function (id, userName) {

        $('#' + id).remove();
    }

    // Открываем соединение
    $.connection.hub.start().done(function () {

        var name = $("#UserId").val();         
        chat.server.connect(name);
          
        $('#sendmessage').click(function () {
            // Вызываем у хаба метод Send
            chat.server.send($('#username').val(), $('#message').val(), $('#UserId').val());
            $('#message').val('');
        });

        $("#message").keypress('keypress', function (e) {
        if (e.which == 13) {
            chat.server.send($('#username').val(), $('#message').val(), $('#UserId').val());
            $('#message').val('');
         }
        });

    });        
});


// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
//Добавление нового пользователя
function AddUser(id, name) {

    var userId = $('#hdId').val();

    if (userId != id) {

        $("#chatusers").append('<p id="' + id + '"><b>' + name + '</b></p>');
    }
}