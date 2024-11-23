import { provinces, districts, wards } from "./diachi.js";


function initAddressDropdown() {

    const hoKhauTinh = document.getElementById("hoKhauTinh");
    const hoKhauQuan = document.getElementById("hoKhauQuan");
    const hoKhauXa = document.getElementById("hoKhauXa");


    if (!hoKhauTinh || !hoKhauQuan || !hoKhauXa) return;

    provinces.forEach((province) => {
        const option = document.createElement("option");
        option.value = province.code;
        option.textContent = province.name;
        hoKhauTinh.appendChild(option);
    });


    hoKhauTinh.addEventListener("change", function () {
        const provinceCode = hoKhauTinh.value;
        hoKhauQuan.innerHTML = '<option>-- Chọn Quận/Huyện --</option>';
        hoKhauXa.innerHTML = '<option>-- Chọn Xã/Phường --</option>';

        const filteredDistricts = districts.filter(d => d.province_code === provinceCode);
        filteredDistricts.forEach((district) => {
            const option = document.createElement("option");
            option.value = district.code;
            option.textContent = district.name;
            hoKhauQuan.appendChild(option);
        });
    });


    hoKhauQuan.addEventListener("change", function () {
        const districtCode = hoKhauQuan.value;
        hoKhauXa.innerHTML = '<option>-- Chọn Xã/Phường --</option>';

        const filteredWards = wards.filter(w => w.district_code === districtCode);
        filteredWards.forEach((ward) => {
            const option = document.createElement("option");
            option.value = ward.code;
            option.textContent = ward.name;
            hoKhauXa.appendChild(option);
        });
    });
}


document.addEventListener("DOMContentLoaded", initAddressDropdown);