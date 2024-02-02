function getLimitedNumbers() {

    let inputValue = document.getElementById("numberInput").value;
    window.location = "https://localhost:7127/Numbers/Limit?num=" + inputValue;
}