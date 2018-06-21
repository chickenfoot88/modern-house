//var loginInput = document.createElement('input');
//loginInput.setAttribute('type', 'text');
//loginInput.setAttribute('id', 'login');
//loginInput.setAttribute('placeholder', 'login');
//loginInput.setAttribute('style', 'width:100px');

//var login = document.createElement('div');
//login.setAttribute('class', 'input');
//login.appendChild(loginInput);


//var passwordInput = document.createElement('input');
//passwordInput.setAttribute('type', 'password');
//passwordInput.setAttribute('id', 'password');
//passwordInput.setAttribute('placeholder', 'password');
//passwordInput.setAttribute('style', 'width:100px');

//var password = document.createElement('div');
//password.setAttribute('class', 'input');
//password.appendChild(passwordInput);

//var loginButon = document.createElement('a');
//loginButon.setAttribute('style', 'background-color: #547f00;display: block;text-decoration: none;font-weight: bold;padding: 6px 8px;font-size: 0.9em;color: white;border-radius: 4px;');
//loginButon.setAttribute('href', '#');
//loginButon.setAttribute('onclick', 'loginButtonClick()');
//loginButon.text = 'Login';

//var loginButonEl = document.createElement('div');
//loginButonEl.setAttribute('class', 'input');
//loginButonEl.appendChild(loginButon);

//var apiSelector = document.getElementById('api_selector');
//var apiKey = document.getElementById('input_apiKey');
//var explore = document.getElementById('explore');

//apiKey.setAttribute('style', 'width:0px;padding:0px;border-width: inherit;');
//apiSelector.insertBefore(login, explore.parentElement);
//apiSelector.insertBefore(password, explore.parentElement);
//apiSelector.insertBefore(loginButonEl, explore.parentElement);
//explore.parentElement.remove();

//function loginButtonClick() {
//    var username = document.getElementById('login').value;
//    var password = document.getElementById('password').value;

//    jQuery.post('/token',
//        {
//            grant_type: 'password',
//            username: username,
//            password: password
//        },
//        function(result) {
//            if (result && result.access_token) {
//                window.swaggerUi.api.clientAuthorizations.remove('custom');
//                window.swaggerUi.api.clientAuthorizations.add("custom",
//                    new SwaggerClient.ApiKeyAuthorization("Authorization", "Bearer " + result.access_token, "header"));
//            } else {
//                alert('Произошла ошибка авторизации');
//            }
//        });
//}