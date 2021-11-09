import VueRouter from "vue-router";
import Tests from "./pages/Tests.vue";
import Details from "./pages/Details.vue";

export default new VueRouter({
    routes: [
        {
            path: '/',
            name: 'Tests',
            component: Tests,
            props: true
        },
        {
            path: '/:testId/details',
            name: 'Details',
            component: Details,
            props: true
        }
    ],
    mode: 'history'
})