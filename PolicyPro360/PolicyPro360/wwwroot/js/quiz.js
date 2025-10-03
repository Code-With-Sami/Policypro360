// wwwroot/js/quiz.js
(async function () {
    window.openQuizModal = function () {
        document.getElementById('quiz-modal').style.display = 'block';
        loadQuiz();
    };
    window.closeQuizModal = function () {
        document.getElementById('quiz-modal').style.display = 'none';
    };

    async function getAntiForgeryToken() {
        var el = document.getElementById('__RequestVerificationToken');
        return el ? el.value : '';
    }

    async function loadQuiz() {
        const root = document.getElementById('quiz-root');
        root.innerHTML = '<p>Loading...</p>';
        try {
            const res = await fetch('/Quiz/GetActive');
            if (!res.ok) {
                root.innerHTML = '<p>No active quiz available.</p>';
                return;
            }
            const quiz = await res.json();
            renderQuiz(root, quiz);
        } catch (err) {
            root.innerHTML = '<p>Error loading quiz.</p>';
            console.error(err);
        }
    }

    function renderQuiz(root, quiz) {
        root.innerHTML = '';

        const title = document.createElement('h5');
        title.textContent = quiz.title || 'Quick Quiz';
        root.appendChild(title);

        const form = document.createElement('form');
        form.id = 'quiz-form';

        quiz.questions.forEach((q, idx) => {
            const qDiv = document.createElement('div');
            qDiv.className = 'mb-3';
            const qLabel = document.createElement('label');
            qLabel.textContent = (idx + 1) + '. ' + q.text;
            qDiv.appendChild(qLabel);

            if (q.questionType === 'single') {
                q.options.forEach(opt => {
                    const id = 'q' + q.id + '_opt' + opt.id;
                    const wrapper = document.createElement('div');
                    wrapper.className = 'form-check';
                    const input = document.createElement('input');
                    input.className = 'form-check-input';
                    input.type = 'radio';
                    input.name = 'q_' + q.id;
                    input.value = opt.id;
                    input.id = id;
                    wrapper.appendChild(input);

                    const label = document.createElement('label');
                    label.className = 'form-check-label';
                    label.htmlFor = id;
                    label.textContent = opt.text;
                    wrapper.appendChild(label);

                    qDiv.appendChild(wrapper);
                });
            } else if (q.questionType === 'multi') {
                q.options.forEach(opt => {
                    const id = 'q' + q.id + '_opt' + opt.id;
                    const wrapper = document.createElement('div');
                    wrapper.className = 'form-check';
                    const input = document.createElement('input');
                    input.className = 'form-check-input';
                    input.type = 'checkbox';
                    input.name = 'q_' + q.id;
                    input.value = opt.id;
                    input.id = id;
                    wrapper.appendChild(input);

                    const label = document.createElement('label');
                    label.className = 'form-check-label';
                    label.htmlFor = id;
                    label.textContent = opt.text;
                    wrapper.appendChild(label);

                    qDiv.appendChild(wrapper);
                });
            } else { // numeric / raw
                const input = document.createElement('input');
                input.type = 'text';
                input.className = 'form-control';
                input.name = 'q_' + q.id;
                qDiv.appendChild(input);
            }

            form.appendChild(qDiv);
        });

        const submitBtn = document.createElement('button');
        submitBtn.type = 'button';
        submitBtn.className = 'btn btn-primary';
        submitBtn.textContent = 'Submit';
        submitBtn.addEventListener('click', () => submitQuiz(quiz.id));
        form.appendChild(submitBtn);

        root.appendChild(form);
    }

    async function submitQuiz(quizId) {
        const form = document.getElementById('quiz-form');
        const fd = new FormData(form);
        // Build answers DTO
        const answers = [];
        // find all inputs by name prefix
        const elements = form.querySelectorAll('[name^="q_"]');
        const processed = new Set();
        elements.forEach(el => {
            const qname = el.name;
            if (processed.has(qname)) return;
            processed.add(qname);
            const qid = parseInt(qname.replace('q_', ''));
            const nodes = form.querySelectorAll('[name="' + qname + '"]');
            const optionIds = [];
            let raw = '';
            nodes.forEach(n => {
                if ((n.type === 'checkbox' || n.type === 'radio') && n.checked) optionIds.push(parseInt(n.value));
                else if (n.tagName.toLowerCase() === 'input' && (n.type === 'text' || n.type === 'number')) raw = n.value;
            });
            answers.push({ questionId: qid, optionIds, rawAnswer: raw });
        });

        const payload = { quizId: quizId, answers: answers };

        const token = await getAntiForgeryToken();

        try {
            const res = await fetch('/Quiz/Submit', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': token
                },
                body: JSON.stringify(payload)
            });

            if (!res.ok) {
                alert('Failed to submit quiz');
                return;
            }
            const data = await res.json();
            showResults(data);
        } catch (err) {
            console.error(err);
            alert('Something went wrong');
        }
    }

    function showResults(data) {
        const root = document.getElementById('quiz-root');
        root.innerHTML = '<h5>Recommendations</h5>';
        const scoreDiv = document.createElement('div');
        scoreDiv.innerHTML = '<strong>Category Scores:</strong><br/>' + JSON.stringify(data.scores || data.Scores || {});
        root.appendChild(scoreDiv);

        if ((data.recommended || data.Recommended) && (data.recommended || data.Recommended).length) {
            const recs = data.recommended || data.Recommended;
            recs.forEach(r => {
                const card = document.createElement('div');
                card.className = 'card mb-2';
                const body = document.createElement('div');
                body.className = 'card-body';
                body.innerHTML = `<h6>${r.name || r.Name}</h6>
                                  <p>Premium: ${r.premium || r.Premium}</p>
                                  <p>Reason: ${r.reason || r.Reason}</p>
                                  <a class="btn btn-sm btn-outline-primary" href="/UserHome/ViewPolicyDetails/${r.id}">View Policy</a>`;
                card.appendChild(body);
                root.appendChild(card);
            });
        } else {
            root.appendChild(document.createElement('p')).textContent = 'No matching policies found.';
        }
    }
})();
