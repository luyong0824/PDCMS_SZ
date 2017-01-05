webpackJsonp([9], {
	0: function(e, o, n) {
		n(1), e.exports = n(338)
	},
	338: function(e, o, n) {
		"use strict";

		function t(e) {
			return e && e.__esModule ? e : {
			default:
				e
			}
		}
		function i() {
			r.
		default.ajax({
				url: v.config.url + "WorkFlow/GetTaskToDoMobile",
				type: "post",
				dataType: "json",
				data: {
					pageIndex: 0,
					pageSize: 50,
					sortField: "",
					sortOrder: ""
				},
				beforeSend: function() {
					h.
				default.show()
				},
				success: function(e) {
					e.length > 0 && s(e), "-2" == e.Code && v.helpers.jump2login(), a()
				},
				fail: function(e) {
					console.log(e), console.log("登录失败,请重试")
				},
				complete: function(e) {
					h.
				default.hide()
				}
			})
		}
		function a() {
			r.
		default.ajax({
				url: v.config.url + "/userInfo",
				type: "get",
				dataType: "json",
				data: {
					pageSize: 50
				},
				beforeSend: function() {
					h.
				default.show()
				},
				success: function(e) {
					"0" === e.Code && l(e)
				},
				fail: function(e) {
					console.log(e), console.log("登录失败,请重试")
				},
				complete: function(e) {
					h.
				default.hide()
				}
			})
		}
		function l(e) {
			var o = '<div class="header"><div class="company">单位: ' + e.CompanyName + '</div>\n        <div class="name">姓名: ' + e.FullName + "</div></div>";
			(0, r.
		default)("#header").html(o)
		}
		function s(e) {
			for (var o = [], n = void 0, t = void 0, i = void 0, a = void 0, l = 0; l < e.length; l++) n = e[l].ProfessionName, t = e[l].TaskTypeName, i = e[l].TaskCount, a = e[l].Profession, o.push('<ul class="list-item">\n            <li><a href="javascript:void(0);">' + n + '</a></li>\n            <li><a href="jddjlist.html?Profession=' + a + "&type=" + c(t) + '">' + t + '</a></li>\n            <li><a href="javascript:void(0);">' + i + "</a></li></ul>");
			(0, r.
		default)(".list").append(o.join(""))
		}
		function c(e) {
			switch (e) {
			case "工程进度登记":
				return 2;
			case "项目进度登记":
				return 1
			}
		}
		n(299), n(301), n(339);
		var u = n(312),
			d = t(u),
			f = n(305),
			r = t(f),
			p = n(306),
			h = t(p),
			v = n(310);
		new d.
	default ({
			middle: {
				text: "待办事项",
				click: function() {}
			},
			right: {
				text: "退出",
				click: function() {
					window.location.href = "/PDCMSWeb/MExit"
				}
			}
		}), i()
	},
	339: function(e, o) {}
});