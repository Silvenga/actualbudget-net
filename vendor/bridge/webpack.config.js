/* eslint-disable @typescript-eslint/no-var-requires */
/* eslint-disable no-undef */

const path = require("path");

module.exports = {
    entry: "./index.js",
    mode: "none",
    target: "node",
    externalsPresets: {
        node: true
    },
    output: {
        path: path.resolve("./dist"),
        filename: "index.js",
        libraryTarget: 'commonjs2',
        libraryExport: 'default'
    },
    resolve: {
        extensions: [".js"],
        modules: ["src", "node_modules"].map(x => path.resolve(x)),
    },
    module: {
        rules: [
            {
                test: /\.m?js$/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: [
                            [
                                '@babel/preset-env',
                                {
                                    "exclude": [
                                        "@babel/plugin-transform-regenerator",
                                        "@babel/plugin-transform-async-to-generator"
                                    ],
                                }
                            ]
                        ],
                        plugins: [
                            ["@babel/plugin-transform-modules-commonjs"],
                        ]
                    }
                }
            }
        ]
    },
    plugins: []
}
