<template>
  <header id="header">
    <div class="container px-0">
      <b-navbar
        class="header-border"
        toggleable="lg"
        type="dark"
        variant="info"
      >
        <b-navbar-brand><a @click="onBack">Crawler</a></b-navbar-brand>

        <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
        <div v-if="showSpinner">
          <b-spinner label="Spinning"></b-spinner>
        </div>
        <b-collapse id="nav-collapse" is-nav>
          <b-navbar-nav class="ml-auto">
            <b-nav-form @submit.prevent="onTest">
              <div v-if="status !== null">
                <b-form-invalid-feedback
                  class="validation-tooltip-position"
                  :state="validation"
                  tooltip
                >
                  {{ errorMsg }}
                </b-form-invalid-feedback>
                <b-form-valid-feedback
                  class="validation-tooltip-position"
                  :state="validation"
                  tooltip
                >
                  Test successfully complete
                </b-form-valid-feedback>
              </div>

              <b-form-input
                size="md"
                class="mx-sm-3"
                placeholder="Url"
                v-model.lazy="url"
                @change="onChange"
              ></b-form-input>
              <b-button
                variant="warning"
                size="md"
                class="my-2 my-sm-0"
                type="submit"
                >Test</b-button
              >
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
  props: {
    baseUri: {
      type: String,
      default() {
        return null;
      },
    },
  },
  data() {
    return {
      url: "",
      errorMsg: null,
      status: null,
      showSpinner: false,
    };
  },
  computed: {
    validation() {
      return this.status === null ? null : this.status < 400 ? true : false;
    },
    input() {
      return { userInput: { Url: this.url } };
    },
  },
  methods: {
    onChange() {
      this.status = null;
    },
    onTest() {
      this.showSpinner = true;
      this.runTest();
    },
    onBack() {
      this.$router.back();
    },

    runTest() {
      axios
        .post(this.baseUri + "/tests", { Url: this.url })
        .then((response) => {
          this.status = response.status;
          this.errorMsg = null;
          this.$root.$emit("loadLastTest")
          this.showSpinner = false;
        })
        .catch((error) => {
          if (error.response) {
            this.status = error.response.status;
            this.errorMsg = error.response.data.Error[0];
            this.showSpinner = false;
          }
        });
    },
  },
  watch: {
    url() {
      this.onChange();
    },
    onTest() {
      this.showSpinner = false;
    }
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

.validation-tooltip-position {
  position: relative !important;
  left: 0 !important;
}

.header-border {
  border-bottom: 10px solid #eac962;
}
</style>
