import { provinces, districts, wards } from './diachi.js';

document.addEventListener("DOMContentLoaded", function () {
    const hoKhauTinhSelect = document.getElementById("hoKhauTinh");
    const tamTruTinhSelect = document.getElementById("tamTruTinh");

    // Điền Tỉnh/TP vào dropdown
    provinces.forEach(province => {
        const option = document.createElement("option");
        option.value = province.code;
        option.textContent = province.name;
        hoKhauTinhSelect.appendChild(option);
        tamTruTinhSelect.appendChild(option.cloneNode(true));
    });

    // Lọc Quận/Huyện khi Tỉnh được chọn
    hoKhauTinhSelect.addEventListener("change", function () {
        filterDistricts("hoKhauQuan", hoKhauTinhSelect.value);
    });

    tamTruTinhSelect.addEventListener("change", function () {
        filterDistricts("tamTruQuan", tamTruTinhSelect.value);
    });

    // Lọc Phường/Xã khi Quận/Huyện được chọn
    document.getElementById("hoKhauQuan").addEventListener("change", function () {
        filterWards("hoKhauXa", this.value);
    });

    document.getElementById("tamTruQuan").addEventListener("change", function () {
        filterWards("tamTruXa", this.value);
    });
});

// Hàm lọc Quận/Huyện theo Tỉnh
function filterDistricts(selectId, provinceCode) {
    const districtSelect = document.getElementById(selectId);
    districtSelect.innerHTML = '<option>-- Chọn Quận/Huyện --</option>';

    const filteredDistricts = districts.filter(d => d.province_code === provinceCode);
    filteredDistricts.forEach(district => {
        const option = document.createElement("option");
        option.value = district.code;
        option.textContent = district.name;
        districtSelect.appendChild(option);
    });

    // Reset Phường/Xã khi thay đổi Quận/Huyện
    document.getElementById(selectId.replace('Quan', 'Xa')).innerHTML = '<option>-- Chọn Xã/Phường --</option>';
}

// Hàm lọc Phường/Xã theo Quận/Huyện
function filterWards(selectId, districtCode) {
    const wardSelect = document.getElementById(selectId);
    wardSelect.innerHTML = '<option>-- Chọn Xã/Phường --</option>';

    const filteredWards = wards.filter(w => w.district_code === districtCode);
    filteredWards.forEach(ward => {
        const option = document.createElement("option");
        option.value = ward.code;
        option.textContent = ward.name;
        wardSelect.appendChild(option);
    });
}
