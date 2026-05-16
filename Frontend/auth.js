const API_URL = "https://calculatoor-hphjfqabfcbkc8gr.canadacentral-01.azurewebsites.net/auth";


// THEME TOGGLE
const toggleBtn = document.getElementById("theme-toggle");
const icon = toggleBtn.querySelector("img");
                        
toggleBtn.addEventListener("click", () => {
    const currentTheme = document.documentElement.getAttribute("data-theme");

    if (currentTheme === "dark") {
        document.documentElement.removeAttribute("data-theme");
        icon.src = "moon.png";
    } else {
        document.documentElement.setAttribute("data-theme", "dark");
        icon.src = "sun-icon-30.png";
    }
});


// toggle between login and register page
function toggleAuth() {
    const register = document.querySelector("#register-section");
    const login = document.querySelector("#login-section");
    register.style.display = register.style.display === 'none' ? 'block' : 'none';
    login.style.display = login.style.display === 'none' ? 'block' : 'none';
}

// Register
document.querySelector("#register-form").addEventListener("submit", async(e) =>{
    e.preventDefault();

    const user = document.querySelector("#reg-user").value;
    const pass = document.querySelector("#reg-pass").value;

    try {
        const response = await fetch(`${API_URL}/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Username: user, Password: pass })
        });
        
        if (!response.ok) {
            const err = await response.json();
            alert("Error: "+ err.message);
            return;
        }
        alert("Registration successful! Now please login.")
        toggleAuth();

    } catch (error) {
        console.error("Error registry:", error);
        alert("Error: "+ error.message);
    }

})

// Login
document.querySelector("#login-form").addEventListener("submit", async(e) =>{
    e.preventDefault();

    const user = document.querySelector("#login-user").value;
    const pass = document.querySelector("#login-pass").value;

    try {

        const response = await fetch(`${API_URL}/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Username: user, Password: pass })
        });

        if (!response.ok) {
            const errorText = await response.text(); // Gets "Invalid username or password."
            alert("Login Failed: " + errorText);
            return;
        }

        const data = await response.json();

        localStorage.setItem('currentUserId', data.userId);
        localStorage.setItem('currentUsername', data.username);

        window.location.href = "calculator.html";
        

    } catch (error) {
        console.error("Error login:", error);
    }
    


})