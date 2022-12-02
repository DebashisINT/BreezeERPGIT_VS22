var path = require('path');
const webpack = require('webpack');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
//const CompressionPlugin = require('compression-webpack-plugin');

module.exports = {
     //mode: 'development',
    mode: 'production',
    //devtool: 'inline-source-map',
    context: __dirname,
    //entry: "./app.js", previous
    entry:{
        //vendor: ['@babel/polyfill', 'react', 'react-dom', 'redux', '@babel/preset-react', '@babel/preset-env'],
        client: './app.js',
    },
    output: {
        path: path.resolve(__dirname, 'dist'),
        chunkFilename: "[name].chunk.js",
        filename:"bundle.js",
        //filename:"[name].[chunkhash].bundle.js",
        publicPath: '../../Scripts/React/dist/'
    },
    
    watch:true,
    externals: function (context, request, callback) {
        if (/xlsx|canvg|pdfmake/.test(request)) {
        return callback(null, "commonjs " + request);
        }
        callback();
    },
    module: {
        rules: [
          {
              test: /\.js$/,
              exclude: /(node_modules)/,
              use: {
                  loader: 'babel-loader',
                  options: {
                      presets:
                    ['@babel/preset-env', '@babel/preset-react',
                        {
                            'plugins': [
                                '@babel/plugin-proposal-class-properties', 
                                ["import", {libraryName: "antd", libraryDirectory: "lib", style: "css"}]
                            ], 
                        }
                    ]
                  }
              }
          },
          {
              test: /\.css$/,
              use: ['style-loader', 'css-loader']
          },
        ]
    },
    // optimization: {
    //     splitChunks: {
    //         chunks: 'all',
    //     },
    // },
    plugins: [
        //new BundleAnalyzerPlugin(),
        // Ignore all locale files of moment.js
        new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/)
    ],
    
    // performance: {
    //     hints: "warning",
    //     // Calculates sizes of gziped bundles.
    //     assetFilter: function (assetFilename) {
    //       return assetFilename.endsWith(".js.gz");
    //     },
    //   }
}