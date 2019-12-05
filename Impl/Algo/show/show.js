import vis from 'vis-network'
import json from './json/*.json'
import { renderCharts } from './charts.js'

let $n = document.getElementById('n')
let $p = document.getElementById('p')
let $render = document.getElementById('render')
let $charts = document.getElementById('charts')
let active = "charts"

let n = $n.value
let p = $p.value

// create an array with nodes
const renderGraph = (graph, onlyStat) => {
    const nodes = new vis.DataSet(
        Object.keys(graph).map(x => ({
            id: x,
            label: ` ${x} `,
            shape: "circle",
            font: { face: "Monospace", align: "center" }
        }))
    )

    // create an array with edges
    const edges = new vis.DataSet(
        Object.keys(graph).reduce(
            (acc, v) => {
                const nodes = Object.entries(graph[v]).map(([to, w]) => ({ from: v, to, arrows: 'to', label: ` ${w} `, font: { background: "#f5f5f5" } }))
                return acc.concat(nodes)
            },
            []
        )
    )

    document.getElementById('stat').innerHTML = `nodes: ${nodes.length}<br/> edges: ${edges.length}`

    if (onlyStat) return;
    // create a network
    const container = document.getElementById('graph')
    const data = {
        nodes: nodes,
        edges: edges
    }
    const options = {}
    const net = new vis.Network(container, data, options)
    net.stabilize(1000)
}

const avg = data => data.reduce((acc, x) => acc + x / data.length, 0)

const renderValues = (name, data) => {
    const $host = document.getElementById(name)
    let html = `<h2>${name[0].toUpperCase() + name.substr(1)}</h2><table>`
    data.forEach((item, idx) => {
        html += `<tr><td>${idx}</td><td>${item}</td></tr>`
    })
    const sorted = [...data].sort()
    html += `<tr><td>AVG</td><td>${avg(data)}</td></tr>`
    html += `<tr><td>MED</td><td>${sorted[Math.round(sorted.length / 2)]}</td></tr>`
    html += "</table>"
    $host.innerHTML = html
}

const showCharts = () => {
    active = "charts"
    const ns = [10, 50, 100, 1000, 5000]
    const chartData = {
        datasets: [
            {
                label: 'Levit',
                data: ns.map(n => avg(json[`n-${n}-p-${p}-r`]['L'])),
                backgroundColor: 'rgba(255, 206, 86, 0.2)',
                borderColor: 'rgba(255, 206, 86, 1)'
            },
            {
                label: 'Dijkstra',
                data: ns.map(n => avg(json[`n-${n}-p-${p}-r`]['D'])),
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)'
            },
            {
                label: 'N^2',
                data: ns.map(n => n * n),
                backgroundColor: 'rgba(0, 99, 132, 0)',
                borderColor: 'rgba(0, 99, 132, 1)'
            },
            {
                label: 'N^3',
                data: ns.map(n => n * n),
                backgroundColor: 'rgba(0, 99, 132, 0)',
                borderColor: 'rgba(0, 99, 132, 1)'
            },
            {
                label: 'n log(n)',
                data: ns.map(n => n * Math.log2(n)),
                backgroundColor: 'rgba(0, 99, 132, 0)',
                borderColor: 'rgba(0, 0, 0, 1)'
            },
        ],
        labels: ns
    }

    renderCharts(chartData, p)
}

const update = () => {
    n = $n.value
    p = $p.value

    const data = json[`n-${n}-p-${p}-r`]
    renderValues('levit', data['L'])
    renderValues('dijkstra', data['D'])
    renderGraph(json[`n-${n}-p-${p}-g`], true)

    if (active == 'charts') {
        showCharts()
    }
}

update()
showCharts()

$n.addEventListener("change", update)
$p.addEventListener("change", update)
$render.addEventListener('click', () => {
    active = "graph"
    renderGraph(json[`n-${n}-p-${p}-g`], false)
})
$charts.addEventListener('click', showCharts)

const makeResTable = () => {
    const ns = [10, 50, 100, 1000, 5000]
    const ps = [0.3, 0.5, 0.65, 0.8, 0.9]

    const table = []
    for (let y = 0; y < ps.length; ++y) {
        table.push([])
        for (let x = 0; x < ns.length; ++x) {
            const data = json[`n-${ns[x]}-p-${ps[y]}-r`]["D"]
            const sorted = [...data].sort()
            table[y][x] = sorted[Math.round(sorted.length / 2)].toFixed(1)
        }
    }
    let html = `<table>`
    table.forEach((row) => {
        html += `<tr>`
        row.forEach(item => { html += `<td>${item}</td>` })
        html += '</tr>'
    })
    html += "</table>"
    document.body.innerHTML = html
}

// makeResTable()