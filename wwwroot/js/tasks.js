const todo = '/api/todo';
const userUrl = '/api/user'
let tasks = [];
const token = localStorage.getItem("token");
const Authorization = "Bearer " + token;

getItems();
IsAdmin();


function getItems() {
    fetch(todo, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': Authorization
            },

        })
        .then(response => {
            if (response.status != 200) {
                throw new Error('Failed to fetch data');
            }
            return response.json();
        })
        .then(data => _displayItems(data))
        .catch(error => {
            console.error('Unable to get items.', error);
            window.location.href = "../index.html";
            
        });
}

function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const item = {
        id: 0,
        name: addNameTextbox.value.trim(),
        isDone: false,
        userId: 0

    };

    fetch(todo, {

            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': Authorization
            },
            body: JSON.stringify(item)


        })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${todo}/${id}`, {

            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': Authorization
            },
        })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = tasks.find(item => item.id === id);
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isDone').checked = item.isDone;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        Id: itemId,
        Name: document.getElementById('edit-name').value.trim(),
        IsDone: document.getElementById('edit-isDone').checked,
        UserId: 0

    };
    fetch(`${todo}/${itemId}`, {

            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': Authorization

            },
            body: JSON.stringify(item)
        })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'task' : 'my tasks';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    console.log("data");
    const tBody = document.getElementById('Tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = item.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    tasks = data;
}

const usersButten = () => {
    const linkToUsers = document.getElementById('forAdmin');
    linkToUsers.hidden = false;
}

function IsAdmin() {
    fetch('/Admin', {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': Authorization
            },
            body: JSON.stringify()
        })
        .then(res => {
            if (res.status === 200)
                usersButten();
        })
        .catch()
}

function updateUser() {
    const user = {
        Id: 0,
        Name: document.getElementById('editUser-name').value.trim(),
        Password: document.getElementById('editUser-password').value.trim(),
        Type: null

    };
   alert(user.Name)
    fetch(userUrl, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': Authorization

        },
        body: JSON.stringify(user)
        })
        .catch(error => console.error('Unable to update item.', error));
    closeeditUserInput();
}

function closeeditUserInput() {
    document.getElementById('editUserForm').hidden = true;
}

function _displayUserDetails() {
    const UserDetails = document.getElementById('editUserForm');
    UserDetails.hidden = false;

}