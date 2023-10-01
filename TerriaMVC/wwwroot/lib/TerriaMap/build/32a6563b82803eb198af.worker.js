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
/******/ 	return __webpack_require__(__webpack_require__.s = "./node_modules/string-replace-webpack-plugin/loader.js?id=in1fze8lt!./node_modules/terriajs/buildprocess/removeCesiumDebugPragmas.js!./node_modules/terriajs-cesium/Source/Workers/transferTypedArrayTest.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./node_modules/string-replace-webpack-plugin/loader.js?id=in1fze8lt!./node_modules/terriajs/buildprocess/removeCesiumDebugPragmas.js!./node_modules/terriajs-cesium/Source/Workers/transferTypedArrayTest.js":
/*!********************************************************************************************************************************************************************************************************************!*\
  !*** ./node_modules/string-replace-webpack-plugin/loader.js?id=in1fze8lt!./node_modules/terriajs/buildprocess/removeCesiumDebugPragmas.js!./node_modules/terriajs-cesium/Source/Workers/transferTypedArrayTest.js ***!
  \********************************************************************************************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("// make sure self is defined so that the Dojo build can evaluate this file without crashing.\nif (typeof self === \"undefined\") {\n  //eslint-disable-next-line no-implicit-globals, no-global-assign\n  self = {};\n}\n\nself.onmessage = function (event) {\n  const array = event.data.array;\n  const postMessage = self.webkitPostMessage || self.postMessage;\n\n  try {\n    // transfer the test array back to the caller\n    postMessage(\n      {\n        array: array,\n      },\n      [array.buffer]\n    );\n  } catch (e) {\n    postMessage({});\n  }\n};\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9ub2RlX21vZHVsZXMvc3RyaW5nLXJlcGxhY2Utd2VicGFjay1wbHVnaW4vbG9hZGVyLmpzP2lkPWluMWZ6ZThsdCEuL25vZGVfbW9kdWxlcy90ZXJyaWFqcy9idWlsZHByb2Nlc3MvcmVtb3ZlQ2VzaXVtRGVidWdQcmFnbWFzLmpzIS4vbm9kZV9tb2R1bGVzL3RlcnJpYWpzLWNlc2l1bS9Tb3VyY2UvV29ya2Vycy90cmFuc2ZlclR5cGVkQXJyYXlUZXN0LmpzLmpzIiwic291cmNlcyI6WyJ3ZWJwYWNrOi8vLy4vbm9kZV9tb2R1bGVzL3RlcnJpYWpzLWNlc2l1bS9Tb3VyY2UvV29ya2Vycy90cmFuc2ZlclR5cGVkQXJyYXlUZXN0LmpzPzhhZGQiXSwic291cmNlc0NvbnRlbnQiOlsiLy8gbWFrZSBzdXJlIHNlbGYgaXMgZGVmaW5lZCBzbyB0aGF0IHRoZSBEb2pvIGJ1aWxkIGNhbiBldmFsdWF0ZSB0aGlzIGZpbGUgd2l0aG91dCBjcmFzaGluZy5cbmlmICh0eXBlb2Ygc2VsZiA9PT0gXCJ1bmRlZmluZWRcIikge1xuICAvL2VzbGludC1kaXNhYmxlLW5leHQtbGluZSBuby1pbXBsaWNpdC1nbG9iYWxzLCBuby1nbG9iYWwtYXNzaWduXG4gIHNlbGYgPSB7fTtcbn1cblxuc2VsZi5vbm1lc3NhZ2UgPSBmdW5jdGlvbiAoZXZlbnQpIHtcbiAgY29uc3QgYXJyYXkgPSBldmVudC5kYXRhLmFycmF5O1xuICBjb25zdCBwb3N0TWVzc2FnZSA9IHNlbGYud2Via2l0UG9zdE1lc3NhZ2UgfHwgc2VsZi5wb3N0TWVzc2FnZTtcblxuICB0cnkge1xuICAgIC8vIHRyYW5zZmVyIHRoZSB0ZXN0IGFycmF5IGJhY2sgdG8gdGhlIGNhbGxlclxuICAgIHBvc3RNZXNzYWdlKFxuICAgICAge1xuICAgICAgICBhcnJheTogYXJyYXksXG4gICAgICB9LFxuICAgICAgW2FycmF5LmJ1ZmZlcl1cbiAgICApO1xuICB9IGNhdGNoIChlKSB7XG4gICAgcG9zdE1lc3NhZ2Uoe30pO1xuICB9XG59O1xuIl0sIm1hcHBpbmdzIjoiQUFBQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTsiLCJzb3VyY2VSb290IjoiIn0=\n//# sourceURL=webpack-internal:///./node_modules/string-replace-webpack-plugin/loader.js?id=in1fze8lt!./node_modules/terriajs/buildprocess/removeCesiumDebugPragmas.js!./node_modules/terriajs-cesium/Source/Workers/transferTypedArrayTest.js\n");

/***/ })

/******/ });