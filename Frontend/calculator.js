const API_URL = "https://calculatoor-hphjfqabfcbkc8gr.canadacentral-01.azurewebsites.net/calculator";

// ── Theme toggle ──
const toggleBtn = document.getElementById("theme-toggle");
const icon = toggleBtn ? toggleBtn.querySelector("img") : null;

function applyTheme(theme) {
    if (theme === "dark") {
        document.documentElement.setAttribute("data-theme", "dark");
        if (icon) icon.src = "sun-icon-30.png";
    } else {
        document.documentElement.removeAttribute("data-theme");
        if (icon) icon.src = "moon.png";
    }
    localStorage.setItem("theme", theme);
}

// Restore saved theme on load
applyTheme(localStorage.getItem("theme") || "light");

if (toggleBtn) {
    toggleBtn.addEventListener("click", () => {
        const isDark = document.documentElement.getAttribute("data-theme") === "dark";
        applyTheme(isDark ? "light" : "dark");
    });
}

// ── User info ──
const currentUserId = localStorage.getItem('currentUserId');
const currentUsername = localStorage.getItem('currentUsername');

// Show in calculator title
const titreEl = document.getElementById("titre");
if (titreEl && currentUsername) {
    titreEl.textContent += ` — ${currentUsername}`;
}

// Show/hide connexion & déconnexion based on auth state
function updateAuthNav() {
    const loggedIn = !!localStorage.getItem('currentUserId');
    document.getElementById('btn-connexion').style.display = loggedIn ? 'none' : '';
    document.getElementById('btn-deconnexion').style.display = loggedIn ? '' : 'none';
}
updateAuthNav();

// ── Navigation ──
function showCalculator() {
    document.getElementById("calculator-view").style.display = "";
    document.getElementById("leaderboard-view").style.display = "none";
}

function showLeaderboard() {
    document.getElementById("calculator-view").style.display = "none";
    document.getElementById("leaderboard-view").style.display = "";
    chargerClassement();
}

function deconnexion() {
    localStorage.removeItem("currentUserId");
    localStorage.removeItem("currentUsername");
    window.location.reload();
}

function connexion() {
    // Redirect to login page or show login UI — adjust to your auth flow
    window.location.href = "auth.html";
}

// ── Leaderboard ──
let lbPage = 1;
let lbTimespan = "alltime";
const LB_LIMIT = 10;

function setTimespan(timespan, btn) {
    lbTimespan = timespan;
    lbPage = 1;
    document.querySelectorAll(".lb-filter").forEach(b => b.classList.remove("active"));
    btn.classList.add("active");
    chargerClassement();
}

function changePage(delta) {
    lbPage = Math.max(1, lbPage + delta);
    chargerClassement();
}

async function chargerClassement() {
    const list = document.getElementById("liste-leaderboard");
    const prevBtn = document.getElementById("lb-prev");
    const nextBtn = document.getElementById("lb-next");
    const pageIndicator = document.getElementById("lb-page-indicator");

    list.innerHTML = `<li class="lb-loading">Chargement…</li>`;

    try {
        const res = await fetch(`${API_URL.replace('/calculator', '')}/api/leaderboard?limit=${LB_LIMIT}&page=${lbPage}&timespan=${lbTimespan}`);
        const data = await res.json();

        const medals = ["🥇", "🥈", "🥉"];
        const offset = (lbPage - 1) * LB_LIMIT;

        if (!data.length) {
            list.innerHTML = `<li class="lb-empty">Aucun résultat</li>`;
        } else {
            list.innerHTML = data.map((entry, i) => {
                const rank = offset + i + 1;
                const medalOrRank = medals[rank - 1] ?? `#${rank}`;
                const isCurrentUser = entry.userId === parseInt(localStorage.getItem('currentUserId'));
                return `<li class="${isCurrentUser ? 'lb-current-user' : ''}">
                    <span class="lb-rank ${rank <= 3 ? 'top' + rank : ''}">${medalOrRank}</span>
                    <span class="lb-name">${entry.username}${isCurrentUser ? ' <span class="lb-you">(vous)</span>' : ''}</span>
                    <span class="lb-count">${entry.expressionCount} calc.</span>
                </li>`;
            }).join("");
        }

        // Pagination controls
        prevBtn.disabled = lbPage === 1;
        nextBtn.disabled = data.length < LB_LIMIT;
        pageIndicator.textContent = `Page ${lbPage}`;

    } catch {
        list.innerHTML = `<li class="lb-empty">Classement non disponible</li>`;
    }

    // Load current user's rank
    const userId = localStorage.getItem('currentUserId');
    if (userId) {
        try {
            const rankRes = await fetch(`${API_URL.replace('/calculator', '')}/api/leaderboard/user/${userId}`);
            if (rankRes.ok) {
                const rankData = await rankRes.json();
                const card = document.getElementById("user-rank-card");
                document.getElementById("user-rank-text").textContent =
                    `Votre rang : #${rankData.rank} — ${rankData.totalExpressions} calcul${rankData.totalExpressions !== 1 ? 's' : ''}`;
                card.style.display = "";
            }
        } catch { /* silently skip */ }
    }
}

let expr = "";

function refresh() {
    document.getElementById("expression").innerText = expr || "";
}

function append(ch) {
    document.getElementById("res").classList.remove("error-text");

    const operators = ["+", "-", "*", "/", "^"];
    const lastChar = expr.slice(-1);

    // Prevent double operators
    if (operators.includes(ch) && operators.includes(lastChar)) {
        return;
    }
    expr += ch;
    refresh();
}

function clearAll() {
    expr = "";
    document.getElementById("res").innerText = "";
    document.getElementById("status").innerText = "";
    refresh();
}

function backspace() {
    expr = expr.slice(0, -1);
    refresh();
}

function applySquare() {
    if (!expr) return;
    expr = `(${expr})^2`;
    refresh();
}

function applySqrt() {
    if (!expr) return;
    expr = `sqrt(${expr})`;
    refresh();
}

function applyExpN() {
    if (!expr) return;
    expr += "^";
    refresh();
}

async function faireCalcul() {
    if (!expr) return;
    const statusEl = document.getElementById("status");
    const resEl = document.getElementById("res");

    // Grab the logged-in User ID from localStorage
    const currentUserId = localStorage.getItem('currentUserId');

    resEl.classList.remove("error-text");
    resEl.innerText = "";

    const operators = ["+", "-", "*", "/", "^"];
    if (operators.includes(expr.slice(-1))) {
        resEl.innerText = "Error: Incomplete expression";
        resEl.classList.add("error-text");
        statusEl.innerText = "";
        return;
    }

    // ─── SI L'UTILISATEUR N'EST PAS CONNECTÉ ───
    if (!currentUserId) {
        try {
            // Remplacer le symbole ^ par ** pour que JavaScript comprenne la puissance
            let jsExpr = expr.replace(/\^/g, "**");
            
            // Calculer directement en JavaScript local
            let localResult = eval(jsExpr);
            
            resEl.innerHTML = localResult;
            statusEl.innerText = ""; // Pas d'historique à charger
            return; // On arrête la fonction ici !
        } catch (err) {
            resEl.innerText = "Error";
            resEl.classList.add("error-text");
            statusEl.innerText = "";
            return;
        }
    }

    // ─── SI L'UTILISATEUR EST CONNECTÉ (Votre code API d'origine) ───
    try {
        const response = await fetch(`${API_URL}/calculer`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Expression: expr, UserId: currentUserId})
        });

        const text = await response.text();
        const data = JSON.parse(text);
        const finalResult = data.res ?? data.result ?? data;
        resEl.innerHTML = finalResult;

        if (typeof finalResult === "string" && (finalResult.includes("Error") || finalResult.includes("Invalid"))) {
            resEl.classList.add("error-text");
        }

        statusEl.innerText = "";
        chargerHistorique(); // Met à jour l'historique de l'utilisateur
    } catch (err) {
        resEl.classList.add("error-text");
        statusEl.innerText = "";
    }
}

async function supprimerLog(id) {
    console.log("Attempting to delete log from DOM with ID:", id);
    
    // Select the element from the DOM using the ID
    const li = document.querySelector(`[data-id="${id}"]`);
    if (li) {
        li.remove(); // Removes it from the screen immediately
    } else {
        console.warn(`Could not find DOM element with data-id="${id}"`);
    }

    try {
        const response = await fetch(`${API_URL}/historique/${id}`, {
            method: "DELETE"
        });

        if (!response.ok) {
            console.error("Erreur suppression serveur:", response.status);
            chargerHistorique(); // Reload if the server deletion failed to sync
        }
    } catch (error) {
        console.error("Erreur suppression:", error);
        chargerHistorique();
    }
}
async function chargerHistorique() {
    const currentUserId = localStorage.getItem('currentUserId');
    const liste = document.getElementById("liste-historique"); 

    if (!currentUserId) {
        liste.innerHTML = "<li><span>Veuillez vous connecter pour voir l'historique.</span></li>";
        return;
    }
    
    try {
        // Using the updated endpoint routing that fixed your 404 error
        const response = await fetch(`${API_URL}/historique/${currentUserId}`)
        const logs = await response.json();
        liste.innerHTML = "";

        logs.forEach(log => {
            // SAFE CHECK: Fallback if your backend uses Capitalized "Id" vs lowercase "id"
            const logId = log.id ?? log.Id;

            const li = document.createElement("li");
            li.setAttribute("data-id", logId); // Attaches the proper ID to the HTML element

            const left = document.createElement("span");
            left.innerHTML = `${log.expression} <span class="eq">= ${log.result}</span>`;

            const right = document.createElement("span");
            right.className = "date";
            right.innerText = new Date(log.createdAt ?? log.CreatedQty ?? Date.now()).toLocaleString();

            const deleteBtn = document.createElement("button");
            deleteBtn.innerText = "✕";
            deleteBtn.className = "btn-delete-log";
            
            // Explicitly pass the correct fixed logId into the click event
            deleteBtn.onclick = () => supprimerLog(logId);

            const rightGroup = document.createElement("div");
            rightGroup.className = "log-right";
            rightGroup.appendChild(right);
            rightGroup.appendChild(deleteBtn);

            li.appendChild(left);
            li.appendChild(rightGroup);
            liste.appendChild(li);
        });
    } catch (err) {
        console.error("Error building history view:", err);
        liste.innerHTML = "<li><span>Historique non disponible</span></li>";
    }
}

window.onload = chargerHistorique;