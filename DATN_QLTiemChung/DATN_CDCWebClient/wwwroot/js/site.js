import { provinces, districts, wards } from "./diachi.js";

function clearDropdown(dropdown) {
    while (dropdown.options.length > 0) {
        dropdown.remove(0);
    }
}

function setDefaultOption(dropdown, defaultText) {
    const defaultOption = document.createElement("option");
    defaultOption.value = ""; 
    defaultOption.textContent = defaultText;
    defaultOption.selected = true;
    defaultOption.disabled = true;
    defaultOption.disabled = true;
    dropdown.appendChild(defaultOption);
}

function selectOption(dropdown, value) {
    for (let option of dropdown.options) {
        if (option.value === value) {
            option.selected = true;
            return;
        }
    }
}

function populateDropdown(dropdown, items, valueField, textField) {
    const firstOptionValue = dropdown.options[0].value;

    items.forEach((item) => {
        const option = document.createElement("option");
        option.value = item[valueField];
        option.textContent = item[textField];

        if (firstOptionValue !== item[valueField]) {
            dropdown.appendChild(option); 
        } else {
     
            dropdown.remove(0);  

            option.selected = true;  
            dropdown.appendChild(option); 
        }
    });
}


function initAddressDropdown() {
    const hoKhauTinh = document.getElementById("hoKhauTinh");
    const hoKhauQuan = document.getElementById("hoKhauQuan");
    const hoKhauXa = document.getElementById("hoKhauXa");

    if (!hoKhauTinh || !hoKhauQuan || !hoKhauXa) return;

    const firstHoKhauTinh = hoKhauTinh.options[0].value;
    const firstHoKhauQuan = hoKhauQuan.options[0].value;
    const firstHoKhauXa = hoKhauXa.options[0].value;


    populateDropdown(hoKhauTinh, provinces, "code", "name");

    if (firstHoKhauTinh !== null) {
        const filteredDistricts = districts.filter(d => d.province_code === firstHoKhauTinh);
        populateDropdown(hoKhauQuan, filteredDistricts, "code", "name");
    }
    if (firstHoKhauQuan !== null) {
        const filteredWards = wards.filter(w => w.district_code === firstHoKhauQuan);
        populateDropdown(hoKhauXa, filteredWards, "code", "name");
    }

    hoKhauTinh.addEventListener("change", function () {
        clearDropdown(hoKhauQuan);
        clearDropdown(hoKhauXa);
        setDefaultOption(hoKhauQuan, "Chọn Quận/Huyện");
        setDefaultOption(hoKhauXa, "Chọn Xã/Phường");

        const provinceCode = hoKhauTinh.value;
        const filteredDistricts = districts.filter(d => d.province_code === provinceCode);

        populateDropdown(hoKhauQuan, filteredDistricts, "code", "name");
    });

    hoKhauQuan.addEventListener("change", function () {
        clearDropdown(hoKhauXa);
        setDefaultOption(hoKhauXa, "Chọn Xã/Phường");
        const districtCode = hoKhauQuan.value;
        const filteredWards = wards.filter(w => w.district_code === districtCode);

        populateDropdown(hoKhauXa, filteredWards, "code", "name");
    });
}

function autoSelectAddress(wardCode) {
    const hoKhauTinh = document.getElementById("hoKhauTinh");
    const hoKhauQuan = document.getElementById("hoKhauQuan");
    const hoKhauXa = document.getElementById("hoKhauXa");

    // Xác định thông tin từ mã Xã/Phường
    const selectedWard = wards.find(w => w.code === wardCode);
    if (!selectedWard) {
        console.log("Không tìm thấy Xã/Phường.");
        console.error("Không tìm thấy Xã/Phường.");
        return;
    }

    const selectedDistrict = districts.find(d => d.code === selectedWard.district_code);
    const selectedProvince = provinces.find(p => p.code === selectedDistrict.province_code);

    if (!selectedDistrict || !selectedProvince) {
        console.error("Không tìm thấy Quận/Huyện hoặc Tỉnh.");
        return;
    }

    // Chọn Tỉnh
    selectOption(hoKhauTinh, selectedProvince.code);

    // Cập nhật dropdown Quận/Huyện
    clearDropdown(hoKhauQuan);
    const filteredDistricts = districts.filter(d => d.province_code === selectedProvince.code);
    populateDropdown(hoKhauQuan, filteredDistricts, "code", "name");
    selectOption(hoKhauQuan, selectedDistrict.code);

    // Cập nhật dropdown Xã/Phường
    clearDropdown(hoKhauXa);
    const filteredWards = wards.filter(w => w.district_code === selectedDistrict.code);
    populateDropdown(hoKhauXa, filteredWards, "code", "name");
    selectOption(hoKhauXa, wardCode);
}

function selectProvince(provinceCode) {
    const hoKhauTinh = document.getElementById("hoKhauTinh");

    const selectedProvince = provinces.find(p => p.code === provinceCode);
    if (!selectedProvince) {
        console.error("Không tìm thấy Tỉnh.");
        return;
    }

    selectOption(hoKhauTinh, selectedProvince.code);

    hoKhauTinh.dispatchEvent(new Event("change"));
}

function selectDistrict(districtCode) {
    const hoKhauQuan = document.getElementById("hoKhauQuan");

    const selectedDistrict = districts.find(d => d.code === districtCode);
    if (!selectedDistrict) {
        console.error("Không tìm thấy Quận/Huyện.");
        return;
    }

    selectOption(hoKhauQuan, selectedDistrict.code);

    hoKhauQuan.dispatchEvent(new Event("change"));
}


function selectWard(wardCode) {
    const hoKhauXa = document.getElementById("hoKhauXa");

    const selectedWard = wards.find(w => w.code === wardCode);
    if (!selectedWard) {
        console.error("Không tìm thấy Xã/Phường.");
        return;
    }

    // Select the ward in the dropdown
    selectOption(hoKhauXa, selectedWard.code);
}


document.addEventListener("DOMContentLoaded", () => {
    initAddressDropdown();

    
    
        



});
