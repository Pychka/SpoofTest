let link = 'Student';

document.getElementById('loginForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    
    const formData = new FormData(e.target);
    const data = Object.fromEntries(formData);
    if(!data.password || !data.login)
    {
        showError('Заполните все поля!');
        return;
    }
    try {
        const response = await fetch(`https://localhost:7221/api/Auth/Enter/${link}?login=${data.login}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data.password)
        });
        if (response.ok) {
            const studentData = await response.json();
            localStorage.setItem("student-me", JSON.stringify(studentData));
            document.location.href = 'https://localhost:7221/mainStudent.html';
        } else {
            alert(await response.text());
            const errorText = await response.text();
            showError(errorText || 'Неверный логин или пароль');
        }
            
    } catch (error) {
        showError('Сервер сдох, попробуй позже');
    }
});

function change(text){
    text.innerText = text.innerText == 'Я преподаватель' ? 'Я студент' : 'Я преподаватель';
    link = text.innerText == 'Я преподаватель' ? 'Teacher' : 'Student';
}

function showError(error){
    const errorDiv = document.getElementById('error-message');
    const message = document.getElementById('message');
    message.textContent = error;
    errorDiv.style.display = 'inline-flex';
    setTimeout(() => {
        errorDiv.style.display = 'none';
    }, 5000);
}