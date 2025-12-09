//Gauge Manager is a class used to create and update gauges accross multiple pages
window.GaugeManager = (function () {

    const gauges = {}; //stores all created gauges

    //This plugin personalize the chart by adding the needle of the gauge
    const cursorPlugin = {
        id: "cursorPlugin",

        afterDraw(chart, args, pluginOptions) {
            const ctx = chart.ctx;

            const value = pluginOptions.value ?? 0;  //value of the needle
            const xScale = chart.scales.x;
            const yScale = chart.scales.y;

            //exact position in pixels
            const x = xScale.getPixelForValue(value);
            const top = yScale.top;
            const bottom = yScale.bottom;

            ctx.save();
            ctx.strokeStyle = pluginOptions.color || "black";
            ctx.lineWidth = pluginOptions.width ?? 4;
            ctx.beginPath();
            ctx.moveTo(x, top);
            ctx.lineTo(x, bottom);
            ctx.stroke();
            ctx.restore();
        }
    };

    //Creates a new gauge on the specified canvas element. Initializes the gauge with a personalized gradient color bar and sets its value to the current value stored in RedFlagsStore
    function createGauge(canvasId, valueElemId) {
        const canvas = document.getElementById(canvasId);

        if (!canvas) return;

        const context = canvas.getContext('2d');
        const gradient = context.createLinearGradient(0, 0, canvas.width, 0);

        gradient.addColorStop(0.00, '#28a745'); // green
        gradient.addColorStop(0.05, '#28a745');
        gradient.addColorStop(0.05, '#ffc107'); // yellow  at 5%
        gradient.addColorStop(0.10, '#ffc107');
        gradient.addColorStop(0.10, '#fd7e14'); // orange at 10%
        gradient.addColorStop(0.20, '#fd7e14');
        gradient.addColorStop(0.20, '#dc3545'); // red at 20%
        gradient.addColorStop(1.00, '#dc3545');

        const gaugeChart = new Chart(context, {
            type: 'bar',
            data: {
                labels: [''],
                datasets: [
                    {
                        data: [100],
                        backgroundColor: gradient,
                        order: 2
                    }
                ]
            },
            options: {
                indexAxis: 'y',
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    x: { min: 0, max: 100, display: false },
                    y: { display: false }
                },
                plugins: {
                    legend: { display: false },
                    tooltip: { enabled: false },
                    cursorPlugin: {
                        value: 0,
                        color: '#000'
                    }
                }
            },
            plugins: [cursorPlugin]
        });
        gauges[canvasId] = {
            gaugeChart,
            valueElem: valueElemId ? document.getElementById(valueElemId) : null,
            currentProbability: 0
        };

        updateGauge(canvasId, RedFlagsStore.get())
    }

    //Animates the gauge needle from its current position to a target value with a constant step (that can be changed)
    //This method is used internally by updateGauge.
    function moveNeedle(canvasId, targetValue, step = 0.5) {
        const gauge = gauges[canvasId];
        if (!gauge) return;

        const chart = gauge.gaugeChart;
        let current = gauge.currentProbability;
        const target = Math.max(0, Math.min(100, targetValue));
        const direction = target > current ? 1 : -1;

        function animate() {
            if (current === target) return;
            current += direction * step;
            if ((direction === 1 && current > target) || (direction === -1 && current < target)) {
                current = target;
            }

            chart.options.plugins.cursorPlugin.value = current;
            chart.update({ duration: 0 });

            if (gauge.valueElem) {
                gauge.valueElem.textContent = current.toFixed(1) + ' %';
            }

            if (current !== target) requestAnimationFrame(animate);
            gauge.currentProbability = current;
        }

        requestAnimationFrame(animate);
    }

    //Updates the gauge’s needle and numeric display to the specified value. This triggers a smooth animation from the current value to the new value
    function updateGauge(canvasId, newValue) {
        moveNeedle(canvasId, newValue);
    }
    return { createGauge, updateGauge }; //expose public
})();