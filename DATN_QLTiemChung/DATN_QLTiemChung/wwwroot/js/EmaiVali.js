function validateEmail() {
    var email = document.getElementById('email').value.trim();  // Lấy giá trị email từ input sử dụng id của nó
    var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,9}$/;  // Biểu thức chính quy kiểm tra email
    var errorElement = document.getElementById('emailError');  // Lấy thẻ span thông báo lỗi sử dụng id của nó
    if (!email) {
        errorElement.textContent = "Email không được để trống.";
        errorElement.style.display = "inline";  // Hiển thị thông báo lỗi
        document.getElementById('email').classList.add('input-error');
    }
    // Kiểm tra định dạng emai
    else if (!emailPattern.test(email)) {
            errorElement.textContent = "Email không hợp lệ! Hãy nhập đúng định dạng (example@gmail.com).";
            errorElement.style.display = "inline";  // Hiển thị thông báo lỗi
            document.getElementById('email').classList.add('input-error');
        }  // Kiểm tra định dạng email
    else {
        errorElement.textContent = "";  // Xóa thông báo lỗi khi email hợp lệ
        errorElement.style.display = "none";   // Ẩn thông báo lỗi
        document.getElementById('email').classList.remove('input-error');
    }
}
function validateEmailSumit() {
    var email = document.getElementById('email').value.trim();  // Lấy giá trị email từ input sử dụng id của nó
    var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,9}$/;  // Biểu thức chính quy kiểm tra email
    var errorElement = document.getElementById('emailError');  // Lấy thẻ span thông báo lỗi sử dụng id của nó

    // Kiểm tra nếu email trống
    if (!email) {
        errorElement.textContent = "Email không được để trống.";
        errorElement.style.display = "inline";  // Hiển thị thông báo lỗi
        document.getElementById('email').classList.add('input-error');
        return false;  // Trả về false nếu email không hợp lệ
    }
    // Kiểm tra định dạng email
    else if (!emailPattern.test(email)) {
        errorElement.textContent = "Email không hợp lệ! Hãy nhập đúng định dạng (example@gmail.com).";
        errorElement.style.display = "inline";  // Hiển thị thông báo lỗi
        document.getElementById('email').classList.add('input-error');
        return false;  // Trả về false nếu email không hợp lệ
    }
    else {
        errorElement.textContent = "";  // Xóa thông báo lỗi khi email hợp lệ
        errorElement.style.display = "none";   // Ẩn thông báo lỗi
        document.getElementById('email').classList.remove('input-error');
        return true;  // Trả về true nếu email hợp lệ
    }
}
