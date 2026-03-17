(function (w) {

    function Splitter(el, opt) {
        this.el = typeof el === "string" ? document.querySelector(el) : el
        if (!this.el) return
        this.id = this.el.id

        this.o = Object.assign({
            min: 150,
            initial: null,
            store: true,
            storageKey: null
        }, opt || {})

        this.v = this.el.classList.contains("vertical")
        this.p1 = this.el.children[0]
        this.bar = this.el.children[1]
        this.p2 = this.el.children[2]

        this.drag = false
        this.last = null

        this.init()
    }

    /* ================= STORAGE ================= */

    Splitter.prototype.getStorageKey = function () {
        if (this.o.storageKey) return "split:" + this.o.storageKey
        return "split:" + location.pathname + ":" + this.id
    }

    Splitter.prototype.saveState = function () {
        if (!this.o.store) return

        const state = {
            size: this.last || 0,
            collapsed: this.el.classList.contains("collapsed-prev")
                ? "prev"
                : this.el.classList.contains("collapsed-next")
                    ? "next"
                    : null
        }

        localStorage.setItem(this.getStorageKey(), JSON.stringify(state))
    }

    Splitter.prototype.loadState = function () {
        if (!this.o.store) return null

        const raw = localStorage.getItem(this.getStorageKey())
        if (!raw) return null

        try { return JSON.parse(raw) }
        catch { return null }
    }

    /* ================= INIT ================= */

    Splitter.prototype.init = function () {

        const state = this.loadState()

        if (state) {
            this.last = state.size

            if (state.collapsed === "prev") {
                this.el.classList.add("collapsed-prev")
                this.p1.style.flex = "0 0 0"
                this.p2.style.flex = "1 1 100%"
            }
            else if (state.collapsed === "next") {
                this.el.classList.add("collapsed-next")
                this.p1.style.flex = "1 1 100%"
                this.p2.style.flex = "0 0 0"
            }
            else if (state.size) {
                this.set(state.size)
            }
        }
        else if (this.o.initial != null) {
            this.set(this.o.initial)
        }

        this.el.addEventListener("mouseenter", () => this.el.classList.add("active"))
        this.el.addEventListener("mouseleave", () => this.el.classList.remove("active"))

        this.bar.addEventListener("mousedown", (e) => {
            if (e.target && e.target.hasAttribute("data-dir")) return
            this.drag = true
            document.body.style.userSelect = "none"
        })

        document.addEventListener("mouseup", () => {
            this.drag = false
            document.body.style.userSelect = ""
        })

        document.addEventListener("mousemove", (e) => this.onDrag(e))

        this.bar.querySelectorAll('[data-dir]').forEach(el => {
            el.addEventListener("click", (e) => {
                e.stopPropagation()
                this.toggle(el.getAttribute("data-dir"))
            })
        })

        window.addEventListener("resize", () => this.refresh())
    }

    /* ================= DRAG ================= */

    Splitter.prototype.onDrag = function (e) {
        if (!this.drag) return

        const r = this.el.getBoundingClientRect()
        let s = this.v ? (e.clientX - r.left) : (e.clientY - r.top)

        if (s < this.o.min) s = this.o.min

        this.set(s)
        this.saveState()
        this.refresh()
    }

    /* ================= SET SIZE ================= */

    Splitter.prototype.set = function (s) {
        this.last = s
        this.el.classList.remove("collapsed-prev", "collapsed-next")

        this.p1.style.flex = "0 0 " + s + "px"
        this.p2.style.flex = "1 1 auto"
    }

    /* ================= TOGGLE ================= */

    Splitter.prototype.toggle = function (dir) {

        this.el.classList.remove("collapsed-prev", "collapsed-next")

        if (dir === "prev") {
            const size = this.v
                ? this.p1.getBoundingClientRect().width
                : this.p1.getBoundingClientRect().height

            if (size === 0) {
                this.p1.style.flex = "0 0 " + (this.last || this.o.min) + "px"
                this.p2.style.flex = "1 1 100%"
            } else {
                this.last = size
                this.p1.style.flex = "0 0 0"
                this.p2.style.flex = "1 1 100%"
                this.el.classList.add("collapsed-prev")
            }
        }

        if (dir === "next") {
            const size = this.v
                ? this.p2.getBoundingClientRect().width
                : this.p2.getBoundingClientRect().height

            if (size === 0) {
                this.p1.style.flex = "0 0 " + (this.last || this.o.min) + "px"
                this.p2.style.flex = "1 1 100%"
            } else {
                this.last = this.v
                    ? this.p1.getBoundingClientRect().width
                    : this.p1.getBoundingClientRect().height
                this.p1.style.flex = "1 1 100%"
                this.p2.style.flex = "0 0 0"
                this.el.classList.add("collapsed-next")
            }
        }

        this.saveState()
        this.refresh()
    }

    /* ================= DEVEXTREME REFRESH ================= */

    Splitter.prototype.refresh = function () {
        if (window.matchMedia("(max-width: 991.98px)").matches) return

        if (!w.DevExpress) return

        setTimeout(() => {
            $(".dx-datagrid").each(function () {
                const inst = $(this).data("dxDataGrid")
                if (inst) inst.updateDimensions()
            })
        }, 30)
    }

    w.Splitter = Splitter

})(window)


function initSplitterForFourCard(key) {
    if (window.matchMedia("(max-width: 991.98px)").matches) {
        return
    }
    new Splitter("#fourCardMainSplitter", {
        initial: Math.floor(window.innerWidth * 0.75),
        min: 500,
        storageKey: `${key}:Main`
    })

    new Splitter("#fourCardLeftSplitter", {
        initial: Math.floor(window.innerHeight * 0.65),
        min: 200,
        storageKey: `${key}:Left`

    })

    new Splitter("#fourCardRightSplitter", {
        initial: Math.floor(window.innerHeight * 0.48),
        min: 200,
        storageKey: `${key}:Right`

    })
}

function initSplitterForTwoCard(key) {
    if (window.matchMedia("(max-width: 991.98px)").matches) {
        return
    }

    new Splitter("#twoCardMainSplitter", {
        initial: Math.floor(window.innerWidth * 0.25),
        min: 280,
        storageKey: `${key}:TwoCardMain`
    })
}
