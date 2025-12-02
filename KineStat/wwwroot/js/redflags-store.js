window.RedFlagsStore = {
    value: 0,
    listeners: [],

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