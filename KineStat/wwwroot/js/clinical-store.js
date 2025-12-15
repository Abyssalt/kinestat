window.ClinicalProfileStore = {
    values: [0, 0, 0, 0, 0, 0, 0, 0, 0],
    listeners: [],

    set(valuesArray) {
        if (!Array.isArray(valuesArray) || valuesArray.length !== 9) return;

        this.values = [...valuesArray];
        sessionStorage.setItem("clinical9", JSON.stringify(valuesArray));

        this.listeners.forEach(cb => cb(this.values));
    },

    get() {
        const stored = sessionStorage.getItem("clinical9");
        return stored ? JSON.parse(stored) : this.values;
    },

    subscribe(cb) {
        this.listeners.push(cb);
        cb(this.get());
    },

    clear() {
        this.values = [0, 0, 0, 0, 0, 0, 0, 0, 0];
        sessionStorage.removeItem("clinical9");
        this.listeners.forEach(cb => cb(this.values));
    }
};
