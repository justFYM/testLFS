/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "build/";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./node_modules/string-replace-webpack-plugin/loader.js?id=9dljo93a14!./node_modules/terriajs/buildprocess/removeCesiumDebugPragmas.js!./node_modules/terriajs-cesium/Source/Workers/transferTypedArrayTest.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./node_modules/string-replace-webpack-plugin/loader.js?id=9dljo93a14!./node_modules/terriajs/buildprocess/removeCesiumDebugPragmas.js!./node_modules/terriajs-cesium/Source/Workers/transferTypedArrayTest.js":
/*!*********************************************************************************************************************************************************************************************************************!*\
  !*** ./node_modules/string-replace-webpack-plugin/loader.js?id=9dljo93a14!./node_modules/terriajs/buildprocess/removeCesiumDebugPragmas.js!./node_modules/terriajs-cesium/Source/Workers/transferTypedArrayTest.js ***!
  \*********************************************************************************************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("// make sure self is defined so that the Dojo build can evaluate this file without crashing.\nif (typeof self === \"undefined\") {\n  //eslint-disable-next-line no-implicit-globals, no-global-assign\n  self = {};\n}\n\nself.onmessage = function (event) {\n  const array = event.data.array;\n  const postMessage = self.webkitPostMessage || self.postMessage;\n\n  try {\n    // transfer the test array back to the caller\n    postMessage(\n      {\n        array: array,\n      },\n      [array.buffer]\n    );\n  } catch (e) {\n    postMessage({});\n  }\n};\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9ub2RlX21vZHVsZXMvc3RyaW5nLXJlcGxhY2Utd2VicGFjay1wbHVnaW4vbG9hZGVyLmpzP2lkPTlkbGpvOTNhMTQhLi9ub2RlX21vZHVsZXMvdGVycmlhanMvYnVpbGRwcm9jZXNzL3JlbW92ZUNlc2l1bURlYnVnUHJhZ21hcy5qcyEuL25vZGVfbW9kdWxlcy90ZXJyaWFqcy1jZXNpdW0vU291cmNlL1dvcmtlcnMvdHJhbnNmZXJUeXBlZEFycmF5VGVzdC5qcy5qcyIsInNvdXJjZXMiOlsid2VicGFjazovLy8uL25vZGVfbW9kdWxlcy90ZXJyaWFqcy1jZXNpdW0vU291cmNlL1dvcmtlcnMvdHJhbnNmZXJUeXBlZEFycmF5VGVzdC5qcz9hMjQwIl0sInNvdXJjZXNDb250ZW50IjpbIi8vIG1ha2Ugc3VyZSBzZWxmIGlzIGRlZmluZWQgc28gdGhhdCB0aGUgRG9qbyBidWlsZCBjYW4gZXZhbHVhdGUgdGhpcyBmaWxlIHdpdGhvdXQgY3Jhc2hpbmcuXG5pZiAodHlwZW9mIHNlbGYgPT09IFwidW5kZWZpbmVkXCIpIHtcbiAgLy9lc2xpbnQtZGlzYWJsZS1uZXh0LWxpbmUgbm8taW1wbGljaXQtZ2xvYmFscywgbm8tZ2xvYmFsLWFzc2lnblxuICBzZWxmID0ge307XG59XG5cbnNlbGYub25tZXNzYWdlID0gZnVuY3Rpb24gKGV2ZW50KSB7XG4gIGNvbnN0IGFycmF5ID0gZXZlbnQuZGF0YS5hcnJheTtcbiAgY29uc3QgcG9zdE1lc3NhZ2UgPSBzZWxmLndlYmtpdFBvc3RNZXNzYWdlIHx8IHNlbGYucG9zdE1lc3NhZ2U7XG5cbiAgdHJ5IHtcbiAgICAvLyB0cmFuc2ZlciB0aGUgdGVzdCBhcnJheSBiYWNrIHRvIHRoZSBjYWxsZXJcbiAgICBwb3N0TWVzc2FnZShcbiAgICAgIHtcbiAgICAgICAgYXJyYXk6IGFycmF5LFxuICAgICAgfSxcbiAgICAgIFthcnJheS5idWZmZXJdXG4gICAgKTtcbiAgfSBjYXRjaCAoZSkge1xuICAgIHBvc3RNZXNzYWdlKHt9KTtcbiAgfVxufTtcbiJdLCJtYXBwaW5ncyI6IkFBQUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7Iiwic291cmNlUm9vdCI6IiJ9\n//# sourceURL=webpack-internal:///./node_modules/string-replace-webpack-plugin/loader.js?id=9dljo93a14!./node_modules/terriajs/buildprocess/removeCesiumDebugPragmas.js!./node_modules/terriajs-cesium/Source/Workers/transferTypedArrayTest.js\n");

/***/ })

/******/ });