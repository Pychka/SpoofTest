import {test} from './test.js';

export function showTask(tests, container){
    container.innerHTML = `
        ${tests.map(test =>
            `<div>
                <span>
                    <a href="https://localhost:7007/api/Test/One?id=${test.id}">${test.title}</a>
                    <a class="teacher-link">:${test.questions.count()}/${test.limitMinutes}min</a>
                </span>
                <p>Description</p>
            </div>`
        ).join('')}        
    `;
}