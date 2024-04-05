const loginUrl = "/api/login";

// if (localStorage.getItem('token')) {
//     window.location.href = "tasks.html";
// }

function getDetailsForLogin() {
    const name = document.getElementById('name').value.trim();
    const password = document.getElementById('password').value.trim();
    login(name, password);
}

function login(name, password) {
    fetch(loginUrl, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Id: 0,
                Name: name,
                Password: password,
                Type: "user"
            })
        })
        .then(response => response.json())
        .then(data => {
            saveToken(data)
        })

    .catch(error =>
        console.error('Unable to save token.', error));
}

function handleCredentialResponse(response) {
    if (response.credential) {
        var idToken = response.credential;
        var decodedToken = parseJwt(idToken);
        var userId = decodedToken.sub;
        var userName = decodedToken.name;
        login(userName, userId);
    } else {
        alert('Google Sign-In was cancelled.');
    }
}

function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

function saveToken(token) {

    localStorage.setItem("token", token);
    var homePagePath = "tasks.html";
    window.location.href = homePagePath;
}