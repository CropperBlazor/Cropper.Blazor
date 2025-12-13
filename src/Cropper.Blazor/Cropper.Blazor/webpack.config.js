require('webpack');
const path = require('path');
const CleanCSS = require('clean-css')
const CopyPlugin = require('copy-webpack-plugin')
const TerserPlugin = require('terser-webpack-plugin')

module.exports = (env, args) => ({
    resolve: {
        extensions: ['.ts', '.js', '.css'],
        alias: {
            cropperjs: path.resolve(__dirname, 'node_modules/cropperjs/src')
        }
    },
    devtool: args.mode === 'development' ? 'inline-source-map' : 'hidden-source-map',
    module: {
        rules: [{
            test: /\.ts?$/,
            loader: 'ts-loader'
        }]
    },
    entry: {
        "cropperJsInterop": './Cropper/cropperJsInterop.ts'
    },
    output: {
        path: path.join(__dirname, '/wwwroot'),
        filename: '[name].min.js'
    },
    plugins: [new CopyPlugin({
        patterns: [{
            to: 'cropper.min.css',
            from: path.resolve(__dirname, 'node_modules/cropperjs/dist', 'cropper.min.css'),
            transform: content => (new CleanCSS({
                level: 2
            }).minify(content)).styles
        }]
    })],
    optimization: {
        minimize: true,
        minimizer: [new TerserPlugin({
            terserOptions: {
                format: {
                    comments: false,
                },
            },
            extractComments: false,
        })],
    },
});