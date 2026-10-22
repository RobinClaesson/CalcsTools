function getLocalStorageKeys() {
    return Object.keys(localStorage);
}

function getLocalStorageItem(key) {
    return localStorage.getItem(key);
}

function setLocalStorageItem(key, value) {
    return localStorage.setItem(key, value);
}

function removeLocalStorageItem(key) {
    return localStorage.removeItem(key);
}

const clearLocalStorage = function () {
    return localStorage.clear();
};

