<template>
  <div id="app">
    <Header msg="sdsds" isValid="true"/>
    <router-view :baseUri = "baseUri"></router-view>
    <Footer />
  </div>
</template>

<script>
import Header from "./components/Header.vue";
import Footer from "./components/Footer.vue";

export default {
  name: "App",
  components: {
    Header,
    Footer,
  },
  data(){
    return {
      baseUri: 'http://crawler.api.loc/api'
      } 
  },
};
</script>

<style lang="scss">
#app {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #283746;
  margin-top: 60px;
}
html {
  overflow-y: scroll;
}

</style>
