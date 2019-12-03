import vis from 'vis-network'
import graph from './json/graph1.json'

// create an array with nodes
const nodes = new vis.DataSet(
    Object.keys(graph).map(x => ({
        id: x,
        label: ` ${x} `,
        shape: "circle",
        font: { face: "Monospace", align: "center" }
    }))
);



// create an array with edges
const edges = new vis.DataSet(
    Object.keys(graph).reduce(
        (acc, v) => {
            const nodes = Object.entries(graph[v]).map(([to, w]) => ({ from: v, to, arrows: 'to', label: ` ${w} `, font: { background: "white" } }))
            return acc.concat(nodes)
        },
        []
    )
);

document.getElementById('stat').innerHTML = `nodes: ${nodes.length}<br/> edges: ${edges.length}`;

// create a network
const container = document.getElementById('graph');
const data = {
    nodes: nodes,
    edges: edges
};
const options = {};
const net = new vis.Network(container, data, options);
net.stabilize(1000);