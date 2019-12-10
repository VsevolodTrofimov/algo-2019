import Chart from 'chart.js'

export const renderCharts = (data, p) => {
    const $graph = document.getElementById('graph')
    $graph.innerHTML = `<canvas style="width:100%;height:100%" id="chart-ctx" />"`
    const $ctx = document.getElementById('chart-ctx')

    console.log(data)

    var myLineChart = new Chart($ctx.getContext('2d'), {
        type: 'line',
        data: data,
        options: {
            scales: {
                yAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: `AVG MS @ p=${p}`,
                    },
                    type: "linear",
                }],
                xAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: 'n'
                    },
                    type: "linear",
                    max: 0,
                    min: 10000,
                }]
            }
        }
    });
}