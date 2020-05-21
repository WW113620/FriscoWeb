var VuePageComponent = Vue.extend({
    template: '<div class=\"page tc\"> <a href=\"javascript:;\" v-on:click=\"go(1)\" v-if=\"pages.pageIndex>1\">First</a>        <a href=\"javascript:;\" v-on:click=\"go(pages.pageIndex-1)\" v-if=\"pages.pageIndex>1\">Prew</a>        <span v-if=\"pages.previousSpot\" v-on:click=\"go(pages.firstNum-1)\">...</span>        <a href=\"javascript:;\" v-for=\"item in pages.pageList\" v-bind:class=\"{cur:pages.pageIndex==item}\" v-on:click=\"go(item)\">{{item}}</a>        <span v-if=\"pages.nextSpot\" v-on:click=\"go(pages.lastNum+1)\">...</span>        <a href=\"javascript:;\" v-if=\"pages.pageIndex < pages.totalPages\" v-on:click=\"go(pages.pageIndex+1)\">Next</a>        <a href=\"javascript:;\" v-if=\"pages.pageIndex < pages.totalPages\" v-on:click=\"go(pages.totalPages)\">Last</a>    </div>',
    props: ['url','prop'],
    data: function () {
        return {
            pages: [],
        }
    },
    mounted: function () {
        this.go(1);
    },
    methods: {
        go: function (n) {
            this.getData(n);
        },
        getData: function (n) {
            this.prop = this.prop || {};
            this.prop.pageIndex = n;
            this.prop.offset=(n-1)*this.prop.limit;
            var me = this;
            axios.post(this.url, this.prop).then(function (res) {
                me.pages = res.data.pages;
                console.log("me.pages:"+JSON.stringify(me.pages));
                me.$emit("getdata", res.data.rows);
            });
        }
    }
})