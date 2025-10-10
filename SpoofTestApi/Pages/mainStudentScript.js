
import {test} from './test.js';

let conatiner = document.getElementById("main-container");

async function showTasks(){
    try {
        const response = await fetch(`https://localhost:7007/api/Test/Many?studentId=${11}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data.password)
        });
        if (response.ok) {
            let tests = test.parseTestFromJSON(await response.json());
            showTask(tests, conatiner);
        } else {
            const errorText = await response.text();
            showError(errorText || 'Неопознаная ошибка');
        }
            
    } catch (error) {
        showError('Сервер сдох, попробуй позже');
    }
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