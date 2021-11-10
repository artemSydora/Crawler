<template>
  <header id="header">
    <div class="container px-0">
      <b-navbar class="bb" toggleable="lg" type="dark" variant="info">
        <b-navbar-brand href="#">Crawler</b-navbar-brand>

        <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

        <b-collapse id="nav-collapse" is-nav>
          <!-- Right aligned nav items -->
          <b-navbar-nav class="ml-auto">
            <b-nav-form @submit.prevent="onTest">
              <b-button
                size="md"
                class="my-2 my-sm-0"
                type="submit"
                >Test</b-button
              >
              <b-form-input
                size=""
                class="mr-sm-2"
                placeholder="Url"
                :state="validation"
                v-model="userInput.url"
              ></b-form-input>
              <b-form-invalid-feedback :state="validation">
                {{ errorMsg }}
              </b-form-invalid-feedback>
              <b-form-valid-feedback :state="validation">
                Test successfully complete
              </b-form-valid-feedback>
            </b-nav-form>
          </b-navbar-nav>
        </b-collapse>
      </b-navbar>
    </div>
  </header>
</template>
<script>
import axios from "axios";

export default {
  name: "Header",
  props:['baseUri'],
  data() {
    return {
      userInput: { url: "" },
      errorMsg: " "
    };
  },
  computed: {
    validation() {
      return this.errorMsg !== null ? true : false;
    },
  },
  methods: {
    onTest(){
      this.saveResults();
      this.$root.$emit('loadPage');
    },
    saveResults() {
      axios.post(this.baseUri + "/tests", this.userInput).catch((error) => {
        this.errorMsg = error.response.data.Error;
      });
    },
  },
};
</script>

<style>
a.navbar-brand {
  white-space: normal;
  text-align: center;
  word-break: break-all;
}
#header {
  position: fixed;
  left: 0;
  top: 0;
  padding: 0px;
  color: #fff;
  width: 100%;
  z-index: 4;
}

.bb {
  border-bottom: 10px solid #eac962;
}
</style>