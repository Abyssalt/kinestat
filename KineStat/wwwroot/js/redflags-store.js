window.RedFlagsStore = {
    value: 0,
    categories: [0, 0, 0, 0, 0, 0],
    listeners: [],
    categoryListeners: [],

    set(value) {
        const v = Math.max(0, Math.min(100, value));
        this.value = v;
        sessionStorage.setItem("redFlagsValue", v);
        this.listeners.forEach(cb => cb(v));
    },

    get() {
        const stored = sessionStorage.getItem("redFlagsValue");
        return stored ? parseFloat(stored) : this.value;
    },

    subscribe(cb) {
        this.listeners.push(cb);
        cb(this.get()); 
    },

    setCategories(categoriesArray) {
        this.categories = [...categoriesArray];
        sessionStorage.setItem("redFlagsCategories", JSON.stringify(categoriesArray));
        this.categoryListeners.forEach(cb => cb(categoriesArray));
    },

    getCategories() {
        const stored = sessionStorage.getItem("redFlagsCategories");
        return stored ? JSON.parse(stored) : this.categories;
    },

    subscribeCategories(cb) {
        this.categoryListeners.push(cb);
        cb(this.getCategories());
    },

    clear() {
        this.value = 0;
        this.categories = [0, 0, 0, 0, 0, 0];
        sessionStorage.removeItem("redFlagsValue");
        sessionStorage.removeItem("redFlagsCategories");
        this.listeners.forEach(cb => cb(0));
        this.categoryListeners.forEach(cb => cb([0, 0, 0, 0, 0, 0]));
    }
};
document.addEventListener("DOMContentLoaded", function () {
    initializeTabs();
    loadQuestionsByCategory(1);
    initializeGauge();

    RedFlagsStore.subscribe(function (newValue) {
        updateGauge(newValue);
    });
});