<template>
  <header>
    <nav
      class="
        navbar navbar-expand-sm navbar-toggleable-sm navbar-light
        bg-white
        border-bottom
        box-shadow
        mb-5
      "
    >
      <div class="container">
        <button class="btn btn-secondary">Home</button>
        <div>
          <form class="form-inline" method="Post">
            <template v-if="isValid">
              <label class="text-success">Enter a website:</label>
              <input
                class="form-control border-success my-1 mx-sm-4"
                name="url"
              />
              <button class="btn btn-outline-success" type="submit">
                Test
              </button>
            </template>
            <template v-else>
              <label class="text-danger">Enter a website:</label>
              <input
                class="form-control border-danger my-1 mx-sm-4"
                name="url"
                data-toggle="tooltip"
                data-placement="bottom"
                :title="msg"
              />
              <button class="btn btn-outline-danger" type="submit">Test</button>
            </template>
          </form>
        </div>
      </div>
    </nav>
  </header>
</template>
<script>
export default {
  name: "Header",
  props: {
    msg: String,
    isValid: Boolean,
  },
};
</script>
<style>
    a.navbar-brand {
    white-space: normal;
    text-align: center;
    word-break: break-all;
    }
</style>